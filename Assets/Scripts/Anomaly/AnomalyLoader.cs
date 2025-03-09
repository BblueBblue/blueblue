using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utility;
using Random = UnityEngine.Random;

namespace Anomaly
{
    public class AnomalyLoader : MonoBehaviour
    {
        public AnomalyMapHandler beforeProblemMap;
        public AnomalyMapHandler currentProblemMap;
        public AnomalyMapHandler nextProblemMap;
        public List<StageScriptableObject> stages;
        public int stageThreshold;
        private int currentMapIdx;
        private int stageIdx;
        private int stageFloor;
        private int sequenceProblem;
        private bool isLoadNewMap = false;
        private BalancedRandomSelector<int> randomSelector;

        [SerializeField]
        private AnomalyClearDataHandler anomalyDataHandler;
        public UnityEvent onClearGame;
        public UnityEvent onFailGame;

        private void Awake()
        {
            if (stageThreshold == 0)
            {
                stageThreshold = 4;
            }
        }

        public void SetAnomaly(int stageIdx)
        {
            this.stageIdx = stageIdx;
            stageFloor = 1;
            sequenceProblem = 1;
            currentMapIdx = -1;
            LoadProblem(stages[stageIdx].defaultPrefab, false);
            isLoadNewMap = true;
            UnloadProblem();
            
            randomSelector = new BalancedRandomSelector<int>();
            for (int i = 0; i < stages[stageIdx].problems.Count; i++)
            {
                randomSelector.AddItem(i);
            }
        }
        
        [Button]
        public void MakeChoice(bool playerChoice)
        {
            // 왼쪽은 이상현상이 있다는 판단, 오른쪽은 없다는 판단이라고 보면 
            // 정상 맵일 때 왼쪽은 오답, 오른쪽은 정답
            // 이상현상 맵일 때 왼쪽은 정답, 오른쪽은 오답.
            // playerChoice는 이상현상이 있는지 없는지에 대한 선택을 의미.
            if (isLoadNewMap)
            {
                return;
            }
            isLoadNewMap = true;
            
            bool isAnswer = currentMapIdx == -1 ? !playerChoice : playerChoice;
            if (isAnswer)
            {
#if UNITY_EDITOR
                Debug.Log("정답!");
#endif
                //정답을 맞춘경우
                if (currentMapIdx != -1)
                {
                    randomSelector.RemoveRandomItem();
                    anomalyDataHandler.CheckAnomaly(stages[stageIdx].problems[currentMapIdx].anomalyName);
                }
                if (++stageFloor == stageThreshold)
                {
                    // 다음 스테이지 로드
                    stageFloor = 1;
                    // 기본 맵 로드
                    if (++stageIdx == stages.Count)
                    {
                        // TODO 엔딩 출력
#if UNITY_EDITOR
                        Debug.Log("게임 클리어!");
#endif
                        onClearGame.Invoke();
                        return;
                    }

                    randomSelector = new BalancedRandomSelector<int>();
                    for (int i = 0; i < stages[stageIdx].problems.Count; i++)
                    {
                        randomSelector.AddItem(i);
                    }
                    
                    currentMapIdx = -1;
                    sequenceProblem = 0;
                    LoadProblem(stages[stageIdx].defaultPrefab, playerChoice);
                    return;
                }
            }
            else
            {
                // 틀린 경우
#if UNITY_EDITOR
                Debug.Log("오답...");
#endif
                if (stageFloor > 1)
                {
                    stageFloor = 1;
                }
                else if (stageFloor == 1)
                {
                    stageFloor = 0;
                }
                else if (stageFloor == 0)
                {
                    
#if UNITY_EDITOR
                    Debug.Log("게임 오버...");
#endif
                    onFailGame.Invoke();
                    return;
                }
            }

            if (sequenceProblem == 2)
            {
                // 연속 정상 맵 등장
                LoadProblem(GetRandomProblem(), playerChoice);
                sequenceProblem = 0;
                return;
            }

            float prob = Random.Range(0f, 1f);
            if (prob < 0.4f)
            {
                // 정상 챕터 사용
                sequenceProblem++;
                currentMapIdx = -1;
                LoadProblem(stages[stageIdx].defaultPrefab, playerChoice);
            }
            else
            {
                sequenceProblem = 0;
                LoadProblem(GetRandomProblem(), playerChoice);
            }
        }
        
        public void LoadProblem(AnomalyScriptableObject problemData, bool isLeft)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                Debug.Log("현재 게임 뷰가 실행 상태가 아닙니다. 실행 상태에서 이 함수를 호출해 주세요.");
                return;
            }
#endif
            
            // 이상현상 생성. (혹은 가져오기)
            Transform spawnTransform = currentProblemMap.loadTransform;
            GameObject problemMapObject = Instantiate(problemData.problemPrefab, spawnTransform.position, spawnTransform.rotation);
            nextProblemMap = problemMapObject.GetComponent<AnomalyMapHandler>();
            // 이상현상 리셋.
            nextProblemMap.ResetProblem();
            nextProblemMap.choiceTrueCollider.loader = nextProblemMap.choiceFalseCollider.loader = nextProblemMap.unloadCollider.loader = this;
            nextProblemMap.floorText.text = stageFloor.ToString();
            
            // 문 여는 애니메이션 실행.
            currentProblemMap.mainDoor.OpenDoor();
        }
        
        public void LoadProblem(GameObject problemMap, bool isLeft)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                Debug.Log("현재 게임 뷰가 실행 상태가 아닙니다. 실행 상태에서 이 함수를 호출해 주세요.");
                return;
            }
#endif
            // 이상현상 생성. (혹은 가져오기)
            Transform spawnTransform = currentProblemMap.loadTransform;
            GameObject problemMapObject = Instantiate(problemMap, spawnTransform.position, spawnTransform.rotation);
            nextProblemMap = problemMapObject.GetComponent<AnomalyMapHandler>();
            // 이상현상 리셋.
            nextProblemMap.ResetProblem();
            nextProblemMap.choiceTrueCollider.loader = nextProblemMap.choiceFalseCollider.loader = nextProblemMap.unloadCollider.loader = this;
            nextProblemMap.floorText.text = stageFloor.ToString();
            
            // 문 여는 애니메이션 실행.
            var door = currentProblemMap.mainDoor;
            door?.OpenDoor();
        }

        public void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.loadedSceneCount);
        }
        
        [Button]
        public void UnloadProblem()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                Debug.Log("현재 게임 뷰가 실행 상태가 아닙니다. 실행 상태에서 이 함수를 호출해 주세요.");
                return;
            }
#endif
            if (!isLoadNewMap)
            {
                return;
            }
            
            // 문을 닫는 애니메이션 실행
            var door = currentProblemMap.mainDoor;
            door?.CloseDoor();
          
            
            // 필요 없어진 이상현상 오브젝트 제거.
            if (beforeProblemMap != null)
            {
                Destroy(beforeProblemMap.gameObject);
            }
            
            beforeProblemMap = currentProblemMap;
            currentProblemMap = nextProblemMap;
            
            beforeProblemMap.ResetProblem();
            isLoadNewMap = false;
        }
        
        private AnomalyScriptableObject GetRandomProblem()
        {
            // 현재 데이터 중 등장하지 않은 랜덤한 값 출력.
            int randomIdx = randomSelector.GetRandomItem();
            currentMapIdx = randomIdx;
            return stages[stageIdx].problems[randomIdx];
        }
    }

}