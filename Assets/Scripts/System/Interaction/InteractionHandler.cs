using System.Collections;
using System.Collections.Generic;
using com.kleberswf.lib.core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace System.Interaction
{
    // 실제 상호작용 처리를 진행하는 클래스.
    // 상호작용 처리를 진행하는 객체는 반드시 하나만 존재해야함.
    // TODO 모든 키 입력은 이후 InputAction으로 변경해야함. 다만 이는 혼자만할 수 있는 부분이 아니기에 임시로 Keycode 사용.
    public class InteractionHandler : Singleton<InteractionHandler>
    {
        [InfoBox("Persistent값을 true로 설정하지 말아주세요!", InfoMessageType.Warning)]
        [Title("Variables")]
        [SerializeField] 
        private Camera mainCamera;
        [SerializeField] 
        private KeyCode startInteractKeyCode;
        [SerializeField] 
        private KeyCode stopInteractKeyCode;
        [SerializeField] 
        private LayerMask interactionLayer;
        [ShowInInspector]
        private IInteractable currentInteractable;
        private IInteractable selectInteractable;

        [SerializeField] 
        private Image hoverImage;
        
        [InfoBox("상호작용이 일어나는 거리는 0보다 작을 경우 4로 고정.")]
        [SerializeField]
        private float _interactDistance;
        private float interactDistance => _interactDistance;
        
        [Title("Events")]
        // 상호작용 중 공통으로 호출해야할 부분은 여기서 실행.
        // ex) 시간 멈춤 등.
        public UnityEvent onStartInteracting;
        public UnityEvent onStopInteracting;
        
        private void Start()
        {
            base.Persistent = false;
            InitVariables();

            var text = hoverImage.GetComponentInChildren<TMP_Text>();
            if (text != null)
            {
                text.text = startInteractKeyCode.ToString();
            }
            
            hoverImage.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(stopInteractKeyCode))
            {
                StopInteract();
            }
         
            GetInteractableObject();
            if (currentInteractable != null)
            {
                hoverImage.gameObject.SetActive(true);
                if (Input.GetKeyDown(startInteractKeyCode))
                {
                    StartInteract();
                }
            }
            else
            {
                hoverImage.gameObject.SetActive(false);
            }
            
            Debug.DrawLine(mainCamera.transform.position, mainCamera.transform.position + mainCamera.transform.forward * interactDistance, Color.red);
        }

        #region About Interaction Handleing
        
        /// <summary>
        /// 상호작용을 할 수 있는 오브젝트를 찾아서 상호작용 진행.
        /// </summary>
        private void GetInteractableObject()
        {
            // lay를 쏴서 currentInteractable 객체 가져오기.
            RaycastHit hit;
            Vector3 screenCenter = Cursor.lockState == CursorLockMode.Locked
                ? Input.mousePosition
                : new Vector3(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2, 0);
            Ray ray = mainCamera.ScreenPointToRay(screenCenter);
            if (Physics.Raycast(ray, out hit, interactDistance, interactionLayer))
            {
                currentInteractable = hit.transform.GetComponent<IInteractable>();
            }
            else
            {
                currentInteractable = null;
            }
        }

        private void StartInteract()
        {
            if (currentInteractable == null)
            {
                return;
            }

            selectInteractable?.StopInteract();

            currentInteractable.StartInteract();
            selectInteractable = currentInteractable;
            onStartInteracting.Invoke();
        }
        
        private void StopInteract()
        {
            if (selectInteractable == null)
            {
                return;
            }
            
            selectInteractable.StopInteract();
            onStopInteracting.Invoke();
            selectInteractable = null;
        }

        #endregion

        #region Init

        private void InitVariables()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            if (startInteractKeyCode == KeyCode.None)
            {
                startInteractKeyCode = KeyCode.E;
            }

            if (stopInteractKeyCode == KeyCode.None)
            {
                startInteractKeyCode = KeyCode.Escape;
            }
            
            if (interactionLayer == 0)
            {
                interactionLayer = LayerMask.NameToLayer("Interactable");
            }

            if (interactDistance <= 0)
            {
                _interactDistance = 4f;
            }
        }

        #endregion
        
    }
}
