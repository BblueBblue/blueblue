using System.Collections;
using System.Collections.Generic;
using System.Interaction;
using PlayerControl;
using UnityEngine;
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
        private List<GameObject> lightObjects;

        [SerializeField] 
        private AudioClip lightOnSFX;

        [SerializeField] 
        private Image LogoImage;

        [SerializeField] 
        private Sprite changedLogo;
        
        void Start()
        {
            SetMouseCursor(true);
        }

        public void SetMouseCursor(bool visible)
        {
            if (visible)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;    
            }
        }
        
        public void MakePlayerMove()
        {
            jumpScareTriggered.isTriggered = false;
        }
        
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void OnStartGame()
        {
            StartCoroutine(TurnOnLight());
        }

        private IEnumerator TurnOnLight()
        {
            yield return new WaitForSeconds(1f);

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
        }
    }
}