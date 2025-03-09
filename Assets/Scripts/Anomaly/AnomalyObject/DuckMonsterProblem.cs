using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using PlayerControl;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Anomaly.Object
{
    public class DuckMonsterProblem : AnomalyObject
    {
        private Vector3 startPos;
        private NavMeshAgent agent;
        private Transform playerTransform;
        private CinemachineVirtualCamera cvc;
        private bool isTracking;

        [SerializeField] 
        private GameObject jumpScareDuck;

        [SerializeField] 
        private Camera mainCamera;
        
        [SerializeField] 
        private SFXPlayer screamSFXPlayer;
        
        [SerializeField] 
        private SFXPlayer duckSfxPlayer;
        
        [SerializeField] 
        private AudioClip screamSFX;

        [SerializeField] 
        private AudioClip duckSFX;

        [SerializeField] 
        private GameObject blackScreen;
        
        public UnityEvent onCatchPlayer;
        private Sequence jumpScareSequence;
        private void Awake()
        {
            startPos = transform.position;
            agent = GetComponent<NavMeshAgent>();
            onCatchPlayer.AddListener(() =>
            {
                isTracking = false;
            });
            onCatchPlayer.AddListener(() =>
            {
                StartCoroutine(JumpScare());
            });
        }

        private void Start()
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            cvc = playerTransform.GetComponentInChildren<CinemachineVirtualCamera>();
            SoundManager.Instance.PlaySFX(duckSfxPlayer, duckSFX, true);
            base.Start();
        }
        
        [Button]
        protected override void ActivePhenomenon()
        {
            StartCoroutine(TrackPlayer());
        }

        public override void ResetProblem()
        {
            transform.position = startPos;
            blackScreen.SetActive(false);
        }

        IEnumerator TrackPlayer()
        {
            isTracking = true;
            Debug.Log("PlayerPos is " + playerTransform.position);
            while (isTracking)
            {
                agent.SetDestination(playerTransform.position);
                Debug.Log("PlayerPos is " + playerTransform.position);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isTracking && other.CompareTag("Player"))
            {
                // 플레이어 잡음
                onCatchPlayer.Invoke();
            }
        }

        IEnumerator JumpScare()
        {
            Debug.Log("Call");
            // 오리 소리 제거
            duckSfxPlayer.Stop();
            
            // 플레이어 이동 제한 및 카메라 움직임 제한시키기
            playerTransform.GetComponent<JumpScareTriggered>().WhenJumpScareTriggered();
            yield return null;
            
            // 괴물 오리 오브젝트 이동
            jumpScareDuck.transform.position = playerTransform.position + new Vector3(0, 2f, 0) + playerTransform.forward * 0.26f;
            jumpScareDuck.transform.rotation = mainCamera.transform.rotation;
            // jumpScareDuck.transform.rotation = new Quaternion(0f, jumpScareDuck.transform.rotation.y, 0f, 1f);
            jumpScareDuck.transform.Rotate(0,180,0,Space.Self);
            yield return null;
            
            // 점프스케어 연출 진행
            jumpScareSequence = DOTween.Sequence();
            jumpScareSequence.SetAutoKill(false);
            jumpScareSequence
                .Append(jumpScareDuck.transform
                    .DOMoveY(mainCamera.transform.position.y - 0.18f, 0.5f).SetEase(Ease.OutExpo))
                .Append(jumpScareDuck.transform
                    .DORotateQuaternion(playerTransform.rotation * Quaternion.Euler(15, 180, 0), 0.1f)
                    .SetDelay(1f)
                    );
            
            // 카메라 흔들림
            CinemachineBasicMultiChannelPerlin noise = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = 0.2f;
            noise.m_FrequencyGain = 1;
            
            // SFX 출력
            SoundManager.Instance.PlaySFX(screamSFXPlayer, screamSFX, false);
            
            // 암전 게임 오버 연출 진행.
            yield return new WaitForSeconds(1.6f);
            blackScreen.SetActive(true);

            // 암전 이후 게임 재시작
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
