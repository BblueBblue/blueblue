using System;
using System.Collections;
using System.Collections.Generic;
using Anomaly;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Book
{
    public class BookSetter : MonoBehaviour
    {
        [SerializeField] 
        private BookScriptableObject bookData;
        
        private Dictionary<AnomalyType, int> chapterIdx;
        private Dictionary<string, PageSetter> pages;
        private int _numOfPage;
        public int numOfPage => _numOfPage;

        private void Awake()
        {
            chapterIdx = new Dictionary<AnomalyType, int>();
        }

        private void Start()
        {
            InitPages();
            gameObject.SetActive(false);
        }

        private void InitPages()
        {
            pages = new Dictionary<string, PageSetter>();
            PageSetter[] pageSetters = GetComponentsInChildren<PageSetter>();

            float pageNum = 1;
            int pageSetterIdx = 0;
            foreach (var pageList in bookData.pages)
            {
                // 챕터별 페이지 설정
                // 챕터가 포함된 시작 페이지 저장.
                chapterIdx.Add(pageList.Key, Mathf.FloorToInt(pageNum));
                pageNum += (float)pageList.Value.Count / 2;
                // 각 페이지 실제 설정.
                foreach (var pageData in pageList.Value)
                {
                    // 페이지 설정
                    pageSetters[pageSetterIdx].InitPage(pageData);
                    pageSetters[pageSetterIdx].gameObject.name = pageData.anomalyData.anomalyName;
                    pages.Add(pageData.anomalyData.anomalyName, pageSetters[pageSetterIdx]);
                    pageSetterIdx++;
                }
            }
            
            // 전체 페이지 수 설정.
            _numOfPage = Mathf.CeilToInt(pageNum);
        }

        public void InitSaveData(AnomalyClearData data)
        {
            foreach (var dataComponent in data.anomalyClearDictionary)
            {
                if (dataComponent.Value)
                {
                    SetPageActivate(dataComponent.Key);
                }
            }
        }
        
        public void SetPageActivate(string anomalyName)
        {
            if (pages.ContainsKey(anomalyName))
            {
                pages[anomalyName].SetActivate();
            }
        }

        public int GetChapterStartPage(AnomalyType chapter)
        {
            if (chapterIdx.TryGetValue(chapter, out var page))
            {
                return page;
            }

            return -1;
        }
        
#if UNITY_EDITOR
        [Button]
        public void PrintChapterPage()
        {
            Debug.Log("print chapter start page info");
            foreach (var idx in chapterIdx)
            {
                Debug.Log("chapter: " + idx.Key + ", startPage: " + idx.Value);
            }
        }
#endif
    }
}