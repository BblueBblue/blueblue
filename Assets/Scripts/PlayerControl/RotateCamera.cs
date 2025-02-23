using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace PlayerControl
{
    public class RotateCamera : SerializedMonoBehaviour
    {
        [Title("이 코드는 카메라에 달아주세요.")]
        [DetailedInfoBox("카메라가 기생할 몸체", "몸에다 달지 말고, 캐릭터의 눈 부분에 연결하세요.")]
        public Transform playerHead;
        [InfoBox("마우스 감도")]
        //public float mouseSensitivity = 100f;
        public float mouseSensitivityVertical = 100f;
        public float mouseSensitivityHorizontal = 100f;
        private float xRotation = 0f;
        [InfoBox("마우스 상하 조절 제한치 조절")]
        [MinMaxSlider(-180, 180,true)]
        public Vector2 mouse_Up_Down_Restrict = new Vector2(-90, 90);

        private bool jumpScareTriggered = false;
        void Start()
        {
            //mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity", 100f);
            mouseSensitivityHorizontal = PlayerPrefs.GetFloat("HorizontalSensitivity", 100f);
            mouseSensitivityVertical = PlayerPrefs.GetFloat("VerticalSensitivity", 100f);
            //SettingUIManager.Instance.onSensitivityValueChanged.AddListener(SetSensitivity);
            SettingUIManager.Instance.onHorizontalSensitivityValueChanged.AddListener(SetHorizontalSensitivity);
            SettingUIManager.Instance.onVerticalSensitivityValueChanged.AddListener(SetVerticalSensitivity);
        }

        void Update()
        {
            //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityHorizontal * Time.deltaTime;
            //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input. GetAxis("Mouse Y") * mouseSensitivityVertical * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, mouse_Up_Down_Restrict.x, mouse_Up_Down_Restrict.y);

            if (!jumpScareTriggered)
            {
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerHead.Rotate(Vector3.up * mouseX);
            }
        }

        private void SetHorizontalSensitivity(float value)
        {
            mouseSensitivityHorizontal = value;
        }
        private void SetVerticalSensitivity(float value)
        {
            mouseSensitivityHorizontal = value;
        }

        public void WhenJumpScareTriggered()
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            jumpScareTriggered = true;
        }
        public void WhenJumpScareReleased()
        {
            jumpScareTriggered = false;
        }
    }
}
