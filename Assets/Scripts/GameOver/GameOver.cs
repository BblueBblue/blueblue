using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Scenes;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;


namespace gameover
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField]
        private DOTweenAnimation gameOverBack;
        [SerializeField]
        private DOTweenAnimation gameOverImg;
        [SerializeField]
        private GameObject blood;

        [SerializeField]
        private AudioClip fallSound;

        public void StartGameOver()
        {
            StartCoroutine(GameOverStart());
        }

        public void DisPlayGameOver()
        {
            gameOverBack.DOPlay();
        }

        public void AfterImageDisplayed()
        {
            StartCoroutine(BloodEffect());
        }

        public void DisPlayGameOverImage()
        {
            gameOverImg.DOPlay();
        }

        public void ResetScene()
        {
            SceneController.Instance.ResetScene(0);
        }

        IEnumerator GameOverStart()
        {
            yield return new WaitForSeconds(1f);
            DisPlayGameOver();
        }
        IEnumerator BloodEffect()
        {
            SoundManager.Instance.PlaySFX(fallSound);
            yield return new WaitForSeconds(0.2f);
            blood.SetActive(true);
            yield return new WaitForSeconds(1f);
            gameOverImg.DOPlayBackwards();
        }
    }
}