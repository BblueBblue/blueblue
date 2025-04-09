using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
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
        
        private string description;
        
        /// <summary>
        /// PageData를 기반으로 page 정보를 갱신
        /// </summary>
        /// <param name="pageData"></param>
        public void InitPage(PageScriptableObject pageData)
        {
            image.sprite = pageData.photo;
            description = pageData.description;
            StringBuilder stringBuilder = new StringBuilder();

            text.text = GetHideString(description);
        }

        /// <summary>
        /// 기본으로 비활성화 된 Page를 활성화시킴.
        /// </summary>
        public void SetActivate()
        {
            inactiveCover.SetActive(false);
            coverImage.gameObject.SetActive(false);
            text.text = description;
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
    }
}
