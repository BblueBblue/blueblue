using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Object
{
    public class ToyFaceAway : AnomalyObject
    {
        private Transform player; 
        private float updateInterval = 0.02f;
        private bool triggered = false;
        private void Start()
        {
            GameObject playerObject = GameObject.FindWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            
            base.Start();
        }
        

        protected override void ActivePhenomenon()
        {
            triggered = true;
            StartCoroutine(UpdateRotation());
        }
        
        IEnumerator UpdateRotation()
        {
            while (triggered)
            {
                
                Vector3 directionToPlayer = transform.position - player.position;
                
                directionToPlayer.y = 0;
                
                if (directionToPlayer != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                    
                    transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
                }
                
                yield return new WaitForSeconds(updateInterval);
            }
        }
        
        
        public override void ResetProblem()
        {
            triggered = false;
        }

        
    }

}