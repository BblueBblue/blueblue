using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Object
{
    public class EyeTrack : AnomalyObject
    {
        [SerializeField] 
        private List<Transform> eyeObjects;

        private GameObject player;
        private bool isActive;

        private void Awake()
        {
            isActive = false;
        }

        private void Update()
        {
            if (isActive)
            {
                foreach (var eyeObject in eyeObjects)
                {
                    eyeObject.LookAt(player.transform);
                }
            }
        }

        protected override void ActivePhenomenon()
        {
            player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                return;
            }
            
            isActive = true;
        }

        public override void ResetProblem()
        {
            isActive = false;
        }
    }
}
