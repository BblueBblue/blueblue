using System.Collections;
using System.Collections.Generic;
using Cinemachine.PostFX;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Anomaly.Object
{
    public class WaterBlockCollision : AnomalyObject
    {
        [SerializeField] private float increaseRate = 0.04f;
        [SerializeField] private float maxIntensity = 0.4f;

        private Vignette vignette;
        private CinemachineVolumeSettings CVS;
        private bool isPlayerCollide = false;
        private bool isGameOver = false;
        [SerializeField] private GameObject blackScreen;
        [SerializeField] private SFXPlayer sfxPlayer;
        [SerializeField] private SFXPlayer sfxPlayer_IN;
        [SerializeField] private SFXPlayer sfxPlayer_OUT;
        [SerializeField] private AudioClip deadSFX;
        [SerializeField] private AudioClip splashCameInSFX;
        [SerializeField] private AudioClip splashCameOutSFX;
        
        private void Start()
        {
            Init();
            base.Start();
        }

        private void Init()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            CVS = player.GetComponentInChildren<CinemachineVolumeSettings>();
            CVS.m_Profile.TryGet(out vignette);
            vignette.color.value = Color.black;
            vignette.intensity.value = 0.2f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //CVS = other.GetComponentInChildren<CinemachineVolumeSettings>();
                ActivePhenomenon();
                SoundManager.Instance.PlaySFX(sfxPlayer_IN, splashCameInSFX, false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerCollide = false;
                SoundManager.Instance.PlaySFX(sfxPlayer_OUT, splashCameOutSFX, false);
                Debug.Log("escape collision");
            }
        }

        protected override void ActivePhenomenon()
        {
            if (vignette.color.value != Color.red)
            {
                vignette.color.value = Color.red;
            }

            isPlayerCollide = true;
            StartCoroutine(GetHurt());
        }

        private IEnumerator GetHurt()
        {
            while (isPlayerCollide && !isGameOver)
            {
                vignette.intensity.value = Mathf.Clamp(vignette.intensity.value + (increaseRate * Time.deltaTime), 0f,
                    maxIntensity);
                if (vignette.intensity.value >= maxIntensity)
                {
                    isGameOver = true;
                    Debug.Log("GameOver");
                    blackScreen.SetActive(true);
                    SoundManager.Instance.PlaySFX(sfxPlayer, deadSFX, false);
                    vignette.intensity.value = 0.2f;
                    vignette.color.value = Color.black;
                    yield return new WaitForSeconds(1.1f);
                    SceneManager.LoadScene(SceneManager.loadedSceneCount);
                }

                yield return null;
            }
        }

        public override void ResetProblem()
        {
            blackScreen.SetActive(false);
        }
    }
}