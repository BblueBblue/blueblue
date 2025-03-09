using System;
using System.Collections;
using System.Collections.Generic;
using System.Scenes;
using Anomaly;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace EndingHall
{
    public class EndingEffector : MonoBehaviour
    {
        private Volume volumeProfile;

        private Bloom bloom;

        [Title("durations")] 
        [SerializeField]
        private float effectDuration = 3f;

        [SerializeField] 
        private float restartDuration = 3f;
        
        [SerializeField] 
        private DoorController door;

        [SerializeField] 
        private AudioClip endingDialogue;
        
        void Start()
        {
            
            volumeProfile = GameObject.FindObjectOfType<Volume>();
            if(volumeProfile.sharedProfile.TryGet(out bloom))
            {
                bloom.intensity.value = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                door.CloseDoor();
                StartCoroutine(EndingSequence());
            }
        }

        IEnumerator EndingSequence()
        {
            // 빛 연출 진행
            // Bloom 조절
            const float targetValue = 150;
            float interpolation = targetValue / effectDuration;
            float time = 0;
            bloom.threshold.value = 0f;

            while (time < effectDuration)
            {
                time += Time.deltaTime;
                bloom.intensity.value += interpolation * Time.deltaTime;
                yield return null;
            }

            bloom.intensity.value = targetValue;

            // 엔딩 대사 출력
            if (endingDialogue != null)
            {
                SoundManager.Instance.PlaySFX(endingDialogue);

                yield return new WaitForSeconds(endingDialogue.length + restartDuration);
            }
            else
            {
                // 대사를 지정하지 않은 경우, 대기 후 재시작
                yield return new WaitForSeconds(restartDuration);
            }

            // 게임 재시작
            SceneController.Instance.ResetScene(0);
        }
        
        private void OnDestroy()
        {
            // 초기화
            if (bloom != null)
            {
                bloom.threshold.value = 1f;
                bloom.intensity.value = 0;
            }
        }
    }
}