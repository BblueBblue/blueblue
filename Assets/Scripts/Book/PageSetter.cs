using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

namespace Book
{
    public class PageSetter : MonoBehaviour
    {
        [SerializeField] 
        private Image coverImage;
        
        [SerializeField] 
        private Image image;

        [SerializeField] 
        private Text text;

        [SerializeField] 
        private GameObject inactiveCover;

        private LocalizedString descriptionLocalizedString;
        private bool isInitialized = false;
        private bool isActivated = false;

        private void OnDestroy()
        {
            if (isInitialized)
            {
                descriptionLocalizedString.StringChanged -= UpdateDescription;
            }
        }

        /// <summary>
        /// PageData를 기반으로 page 정보를 갱신
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="localizationTableName"></param>
        public void InitPage(PageScriptableObject pageData, string localizationTableName)
        {
            image.sprite = pageData.photo;
            descriptionLocalizedString = new LocalizedString(localizationTableName, pageData.descriptionKey);
            descriptionLocalizedString.StringChanged += UpdateDescription;
            
            text.text = GetHideString(descriptionLocalizedString.GetLocalizedString());
            isInitialized = true;
        }

        /// <summary>
        /// 기본으로 비활성화 된 Page를 활성화시킴.
        /// </summary>
        public void SetActivate()
        {
            isActivated = true;
            inactiveCover.SetActive(false);
            coverImage.gameObject.SetActive(false);
            text.text = descriptionLocalizedString.GetLocalizedString();
        }

        private string GetHideString(string targetString)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var c in targetString)
            {
                stringBuilder.Append(c != ' ' ? '?' : ' ');
            }

            return stringBuilder.ToString();
        }

        private void UpdateDescription(string description)
        {
            if (isActivated)
            {
                text.text = description;
            }
            else
            {
                text.text = GetHideString(description);
            }
        }
        
        [Button]
        private void ForceUnlock()
        {
            SetActivate();
        }
    }
}
