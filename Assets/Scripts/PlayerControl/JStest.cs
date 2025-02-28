using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl {
    public class JStest : MonoBehaviour
    {
        private JumpScareTriggered j;

        public bool change = false;
        private bool jChange = false;
        public bool JChange
        {
            get => jChange;
            set
            {
                if (jChange == value) return;
                jChange = value;
                if (jChange) j.WhenJumpScareTriggered();
                else j.WhenJumpScareReleased();
            }
        }

        private void Start()
        {
            j = GetComponent<JumpScareTriggered>();
        }
        private void Update()
        {
            if(change != jChange)
            {
                JChange = change;
            }
        }
    }
}