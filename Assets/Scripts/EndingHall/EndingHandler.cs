using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace EndingHall
{
    public class EndingEffector : MonoBehaviour
    {
        private Volume volumeProfile;

        private Bloom bloom;
        
        void Start()
        {
            volumeProfile = FindObjectOfType<Volume>();
            if(volumeProfile.sharedProfile.TryGet(out bloom))
            {
                bloom.intensity.value = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(EndingEffect(0.8f));
            }
        }

        IEnumerator EndingEffect(float duration)
        {
            // Bloom 조절
            const float targetValue = 150;
            float interpolation = targetValue / duration;
            float time = 0;
            bloom.threshold.value = 0f;
            
            while (time < duration)
            {
                time += Time.deltaTime;
                bloom.intensity.value += interpolation * Time.deltaTime;
                yield return null;
            }

            bloom.intensity.value = targetValue;
        }
        
        private void OnDestroy()
        {
            // 초기화
            if (bloom != null)
            {
                bloom.threshold.value = 1f;
                bloom.intensity.value = 0;
            }
        }
    }
}