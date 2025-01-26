using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Anomaly.Object
{

    public class GhostHandMove : AnomalyObject
    {
        [SerializeField] private GameObject ghostHand;
        [SerializeField] private float speed = 2f;
        [SerializeField] private float distance;
        private float initZ;
        private bool check = false;
        private void Start()
        {
            initZ = ghostHand.transform.localPosition.z;
            check = false;
            base.Start();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (check != true) 
                    ActivePhenomenon();
            }
        }

        protected override void ActivePhenomenon()
        {
            Debug.Log("Good");
            StartCoroutine(MoveOverTime());
        }
        
        private IEnumerator MoveOverTime()
        {
            while (ghostHand.transform.localPosition.z > initZ + distance)
            {
                ghostHand.transform.localPosition -= Vector3.forward * speed * Time.deltaTime;
                
                yield return null;
            }
            
            //ghostHand.transform.localPosition = new Vector3(transform.localPosition.x, ghostHand.transform.localPosition.y, initZ + distance);
            check = true;
        }
        
        public override void ResetProblem()
        {
            ghostHand.SetActive(false);
        }
    }

}