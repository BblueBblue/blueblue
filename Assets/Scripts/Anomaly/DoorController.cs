using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Anomaly
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        private static readonly int Close = Animator.StringToHash("Close");
        private static readonly int Open = Animator.StringToHash("Open");

        [SerializeField] 
        private AudioClip openSFX;
        [SerializeField] 
        private AudioClip closeSFX;
        
        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        public void OpenDoor()
        {
            Debug.Log("문 열기");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("door_open"))
            {
                return;
            }
            animator.SetTrigger(Open);
            if (openSFX != null)
            {
                SoundManager.Instance.PlaySFX(openSFX);
            }
        }

        public async void CloseDoor()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("door_close"))
            {
                return;
            }
            Debug.Log("문 닫기");
            animator.SetTrigger(Close);
            if (closeSFX != null)
            {
                await Task.Delay(500);
                SoundManager.Instance.PlaySFX(closeSFX);
            }
        }
    }

}