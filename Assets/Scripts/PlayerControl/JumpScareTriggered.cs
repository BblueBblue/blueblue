using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl
{
    public class JumpScareTriggered : MonoBehaviour
    {
        private bool isTriggered = false;
        public GameObject cameraRotated;
        private PlayerController p;
        private RotateCamera r;

        public bool IsTriggered
        {
            get => isTriggered;
            set
            {
                if (isTriggered == value) return;
                isTriggered = value;
                if(isTriggered)
                {
                    WhenJumpScareTriggered();
                }else
                {
                    WhenJumpScareReleased();
                }
            }
        }

        private void Awake()
        {
            r = cameraRotated.GetComponent<RotateCamera>();
            p = GetComponent<PlayerController>();
        }


        public void WhenJumpScareTriggered()
        {
            r.WhenJumpScareTriggered();
            p.WhenJumpScareTriggered();
        }
        public void WhenJumpScareReleased()
        {
            r.WhenJumpScareReleased();
            p.WhenJumpScareReleased();
        }
    }
}
