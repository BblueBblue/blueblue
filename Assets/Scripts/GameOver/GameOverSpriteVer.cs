using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Scenes;

namespace Gameover {
    public class GameOverSpriteVer : MonoBehaviour
    {
        [SerializeField]
        private GameObject back;
        [SerializeField]
        private GameObject gameOver;

        [SerializeField]
        private Material bloodMaterial;
        [SerializeField]
        private float effectTimeCount;

        [SerializeField]
        private AudioClip gameOverSound;

        public bool testing = false;

        private float customTime = 0f;

        private void Update()
        {
            if (testing) StartGameOver();
            customTime += Time.deltaTime;
            bloodMaterial.SetFloat("_CustomTime", customTime);
        }

        public void OnGameOver()
        {
            testing = true;
        }

        public void StartGameOver()
        {
            back.SetActive(true);
        }

        public void OnCompleteSettingBackground()
        {
            gameOver.SetActive(true);
        }

        public void OnCompleteSettingGameOverImage()
        {
            customTime = 0.2f;
            gameOver.GetComponent<SpriteRenderer>().material = bloodMaterial;
            SoundManager.Instance.PlaySFX(gameOverSound);
            StartCoroutine(EndGame());
        }

        IEnumerator EndGame()
        {
            yield return new WaitForSeconds(effectTimeCount);
            gameOver.GetComponent<DOTweenAnimation>().DOPlayBackwards();
        }

        public void ResetScene()
        {
            SceneController.Instance.ResetScene(0);
        }
    } 
}
