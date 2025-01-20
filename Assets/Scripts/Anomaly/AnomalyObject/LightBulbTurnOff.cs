using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Object
{
    public class LightBulbTurnOff : AnomalyObject
    {
        [SerializeField] private Light spotLight;
        [SerializeField] private Light pointLight;
        [SerializeField] private SFXPlayer sfxPlayer;
        [SerializeField] private AudioClip buzzySFX;
        [SerializeField] private AudioClip breakSFX;
        
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private GameObject lamp;
        
        private float offIntensity = 5;  
        private float onIntensitySpot = 50f;
        private float onIntensityPoint = 40f;
        private float duration = 4.104f;  // 4 sec. maybe
        private bool triggered = false;
        
        private Material mat;
        private Color baseEmissionColor;
        private float curIntensity;
        
        private void Start()
        {
            base.Start();
        }

        private void GetEmissionMap(GameObject lamp)
        {
            Renderer renderer = lamp.GetComponent<Renderer>();
            if (renderer != null)
            {
              
                mat = renderer.material; // 런타임 시 복사본을 생성
                mat.EnableKeyword("_EMISSION");

                // base Emission 색상 가져오기
                if (mat.HasProperty("_EmissionColor"))
                {
                    Color emissionColor = mat.GetColor("_EmissionColor");

                    // Emission Color의 Intensity 추출
                    curIntensity = emissionColor.maxColorComponent; 
                    baseEmissionColor = emissionColor / curIntensity; // Intensity 분리
                }
                else
                {
                    baseEmissionColor = Color.white; // 기본
                }
                
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            GetEmissionMap(lamp);
            if (other.CompareTag("Player"))
            {
                ActivePhenomenon();
            }
        }

        protected override void ActivePhenomenon()
        {
            if (!triggered)
            {
                StartCoroutine(FlickerLight());
            }
            
        }
        
        IEnumerator FlickerLight()
        {
            triggered = true;
            SoundManager.Instance.PlaySFX(sfxPlayer, buzzySFX, false);
            
            float elapsedTime = 0.0f;
            bool isOn = false;
            float emissionIntensity = 0.0f;
            
            while (elapsedTime < duration)
            {
                isOn = !isOn;
                spotLight.intensity = isOn ? onIntensitySpot : offIntensity;
                pointLight.intensity = isOn ? onIntensityPoint : offIntensity;
                Color emissionColor =
                    isOn
                        ? baseEmissionColor * Mathf.LinearToGammaSpace(-1.0f)
                        : baseEmissionColor * Mathf.LinearToGammaSpace(0.2f);
                
                mat.SetColor("_EmissionColor", emissionColor);
                elapsedTime += Time.deltaTime;
                yield return null;// 
            }
            
            // intensity를 완전히 0으로 설정
            SoundManager.Instance.PlaySFX(sfxPlayer,breakSFX, false);
            particle.Play();
            yield return new WaitForSeconds(0.05f);
            
            spotLight.intensity = 0.0f;
            pointLight.intensity = 0.0f;
            mat.SetColor("_EmissionColor", Color.black);
        }

        public override void ResetProblem()
        {
            spotLight.intensity = 50.0f;
            pointLight.intensity = 40.0f;
            triggered = false;
        }

    }
}