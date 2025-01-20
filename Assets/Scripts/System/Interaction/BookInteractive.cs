using System.Collections;
using System.Collections.Generic;
using Anomaly;
using Book;
using UnityEngine;
using UnityEngine.UI.Extensions;
using PageSelector = Book.PageSelector;

namespace System.Interaction
{
    public class BookInteractive : MonoBehaviour, IInteractable
    {
        [SerializeField] 
        private PageSelector pageSelector;

        [SerializeField] 
        private AnomalyType anomalyType;

        private void Start()
        {
            // TODO 이 코드는 상당히 안좋음. 일단 급하기 때문에 이렇게 작성. 추후 수정 필요.
            pageSelector = GameObject.Find("AnomalyDataHandler").transform.GetChild(0).GetChild(0).GetComponent<PageSelector>();
        }

        public void StartInteract()
        {
            if (pageSelector == null)
            {
                Debug.LogError("책 인터렉션 오브젝트에 PageSelector가 설정되지 않았습니다.");
            }
            pageSelector.gameObject.SetActive(true);
            pageSelector.SetPageToStartChapter(anomalyType);
        }

        public void StopInteract()
        {
            pageSelector.gameObject.SetActive(false);
        }
    }
}
