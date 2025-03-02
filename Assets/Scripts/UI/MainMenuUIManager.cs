using System;
using System.Collections;
using System.Collections.Generic;
using System.Interaction;
using DG.Tweening;
using PlayerControl;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] 
        private JumpScareTriggered jumpScareTriggered;

        [SerializeField] 
        private GameObject EmissionOffObject;
        [SerializeField] 
        private GameObject EmissionOnObject;
        [SerializeField] 
        private DOTweenAnimation startAnimation;
        
        [SerializeField] 
        private List<GameObject> lightObjects;

        [SerializeField] 
        private AudioClip lightOnSFX;

        [SerializeField] 
        private Image LogoImage;

        [SerializeField] 
        private Sprite changedLogo;

        private bool isStartGame;

        [SerializeField]
        private Volume volumeProfile;
        private Vignette vignette;
        
        private void Awake()
        {
            isStartGame = false;
        }

        IEnumerator Start()
        {
            yield return null;
            if (volumeProfile.sharedProfile.TryGet(out vignette))
            {
                vignette.intensity.value = 0.4f;
            }
            jumpScareTriggered.WhenJumpScareTriggered();
        }

        private void Update()
        {
            if (!isStartGame && !Input.GetKeyDown(KeyCode.Escape) && Input.anyKeyDown)
            {
                isStartGame = true;
                OnStartGame();
            }
        }

        public void OnStartGame()
        {
            startAnimation.DORestartAllById("StartGame");
            StartCoroutine(VignetteRelease(0.4f));
            StartCoroutine(TurnOnLight());
        }

        private IEnumerator VignetteRelease(float duration)
        {
            float interpolation = 0.2f / duration;
            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                vignette.intensity.value -= Time.deltaTime * interpolation;
                yield return null;
            }

            vignette.intensity.value = 0.2f;
        }
        
        private IEnumerator TurnOnLight()
        {
            yield return new WaitForSeconds(2.5f);

            // 전등 오브젝트 활성화
            foreach (var lightObject in lightObjects)
            {
                lightObject.SetActive(true);
            }
            
            // Emission Light 활성화
            EmissionOffObject.SetActive(false);
            EmissionOnObject.SetActive(true);
            
            // SFX 출력
            if (lightOnSFX != null)
            {
                SoundManager.Instance.PlaySFX(lightOnSFX);
            }
            
            // 이미지 변경
            LogoImage.sprite = changedLogo;

            yield return null;
            
            jumpScareTriggered.WhenJumpScareReleased();
        }
    }
}