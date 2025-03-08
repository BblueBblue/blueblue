using System.Collections;
using System.Collections.Generic;
using com.kleberswf.lib.core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        
        [SerializeField] private Image panelImage;
        
        void Start()
        {
            if (panelImage != null)
                panelImage.gameObject.GetComponent<Image>().enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (panelImage != null)
                panelImage.gameObject.GetComponent<Image>().enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (panelImage != null)
                panelImage.gameObject.GetComponent<Image>().enabled = false;
        }
}
    }