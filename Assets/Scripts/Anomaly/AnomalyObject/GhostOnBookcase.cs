using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using PlayerControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anomaly.Object
{
    public class GhostOnBookcase : AnomalyObject
    {
        public Transform player;

        [SerializeField] 
        private Transform ghostHead;

        [SerializeField] 
        private GameObject ghostObject;
        
        [SerializeField] 
        private Transform ghostJumpScare;

        [SerializeField] 
        private GameObject blackScreen;

        [SerializeField] 
        private List<AudioClip> jumpScareClips;
        [SerializeField]
        private List<SFXPlayer> sfxPlayers;
        
        private CinemachineVirtualCamera cvc;
        private Vector3 poolPosition;
        private bool isActive;
        private void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
            cvc = player.GetComponentInChildren<CinemachineVirtualCamera>();
            
            isActive = false;
            poolPosition = new Vector3(-100, 0, 0);
            ghostJumpScare.transform.position = poolPosition;
            base.Start();
        }
        
        private void Update()
        {
            ghostHead.LookAt(player);    
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && !isActive)
            {
                ActivePhenomenon();
            }
        }

        protected override void ActivePhenomenon()
        {
            isActive = true;
            StartCoroutine(JumpScare());
        }

        public override void ResetProblem()
        {
            isActive = false;
            ghostJumpScare.position = poolPosition;
            blackScreen.SetActive(false);
            ghostObject.SetActive(true);
        }
        
        IEnumerator JumpScare()
        {
            Debug.Log("Call");
            Camera mainCamera = Camera.main;
            // 기존 귀신 오브젝트 삭제
            ghostObject.SetActive(false);
            
            // 플레이어 이동 제한 및 카메라 움직임 제한시키기
            player.GetComponent<JumpScareTriggered>().WhenJumpScareTriggered();
            yield return null;
            
            // 귀신 오브젝트 이동
            ghostJumpScare.transform.position = player.position + new Vector3(0, -1.5f, 0) + player.forward * 0.5f;
            ghostJumpScare.transform.rotation = mainCamera.transform.rotation;
            // jumpScareDuck.transform.rotation = new Quaternion(0f, jumpScareDuck.transform.rotation.y, 0f, 1f);
            ghostJumpScare.transform.Rotate(0,180,0,Space.Self);
            yield return null;
            
            // 점프스케어 연출 진행
            Sequence jumpScareSequence = DOTween.Sequence();
            jumpScareSequence.SetAutoKill(false);
            jumpScareSequence
                .Append(ghostJumpScare.transform
                    .DOMoveY(mainCamera.transform.position.y - 3.3f, 0.5f).SetEase(Ease.OutExpo))
                .Play();
            
            // 카메라 흔들림
            CinemachineBasicMultiChannelPerlin noise = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = 0.2f;
            noise.m_FrequencyGain = 1;
            
            // SFX 출력
            for (int i = 0; i < jumpScareClips.Count; i++)
            {
                SoundManager.Instance.PlaySFX(sfxPlayers[i], jumpScareClips[i]);
            }
            
            // 암전 게임 오버 연출 진행.
            yield return new WaitForSeconds(1f);
            blackScreen.SetActive(true);

            // 암전 이후 게임 재시작
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

