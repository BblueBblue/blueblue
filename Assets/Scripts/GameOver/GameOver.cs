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
            gameOverImg.DOPlayBackwards();
        }

        public void DisPlayGameOverImage()
        {
            SoundManager.Instance.PlaySFX(fallSound);
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
    }
}