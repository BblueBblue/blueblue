using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Anomaly.Object
{
    public class ComputerJumpScare : AnomalyObject
    {
        [SerializeField] public UniversalRendererData rendererData;
        [SerializeField] private AudioClip audioclip;
        private bool isTriggered = false;
        private string featureName = "FullScreenPassRendererFeature";
        private ScriptableRendererFeature fullScreenFeature ;

        private void Start()
        {
            base.Start();
        }

        private IEnumerator ToggleRenderFeature(string featureName, bool isActive) //RenderData로부터 Renderer Feature 불러오기
        {
            foreach (var feature in rendererData.rendererFeatures)
            {
                if (feature.name == featureName)
                {
                    feature.SetActive(isActive);
                    Debug.Log($"{featureName} 상태: {isActive}");
                    fullScreenFeature = feature;
                    break;
                }
            }

            yield return null;
        }
        
        protected override void ActivePhenomenon()
        {
            StartCoroutine(AnomalyCoroutine());
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isTriggered)
            {
                isTriggered = true;
                ActivePhenomenon();
            }
        }

        private IEnumerator AnomalyCoroutine()
        {
            // 이상현상 싫행
            StartCoroutine(ToggleRenderFeature(featureName, true));
            SoundManager.Instance.PlaySFX(audioclip);
            yield return new WaitForSeconds(1.5f);

            // 이상현상 종료
            StartCoroutine(ToggleRenderFeature(featureName, false));


        }
        
        public override void ResetProblem()
        {
            fullScreenFeature.SetActive(false);
            isTriggered = false;
        }


    }
}