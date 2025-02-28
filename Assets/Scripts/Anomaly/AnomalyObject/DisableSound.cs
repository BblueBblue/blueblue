using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Object
{
    public class DisableSound : MonoBehaviour
    {
        [SerializeField] private SFXPlayer[] sfxPlayers;
        private bool isTriggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")&& isTriggered == false)
            {
                Array.ForEach(sfxPlayers,MuteSound);
                isTriggered = true;
            }
        }

        private void MuteSound(SFXPlayer sfxPlayer)
        {
            sfxPlayer.Stop();
        }

    }

}