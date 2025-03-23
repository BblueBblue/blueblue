using System;
using System.Collections;
using System.Collections.Generic;
using Anomaly.Object;
using TMPro;
using UnityEngine;

namespace Anomaly
{
    public class AnomalyMapHandler : MonoBehaviour
    {
        public Transform loadTransform;

        public DoorController mainDoor;

        public NextAnomalyChoicer choiceTrueCollider;
        public NextAnomalyChoicer choiceFalseCollider;
        public AnomalyUnLoader unloadCollider;
        
        public AnomalyObject problem;

        public TMP_Text floorText;

        public String SteamAchievement = "NAN";
        
        public void ResetProblem()
        {
            // 이상 현상을 리셋
            if (problem != null)
            {
                problem.ResetProblem();
            }
            Debug.Log("이상 현상 리셋");
        }
    }
}