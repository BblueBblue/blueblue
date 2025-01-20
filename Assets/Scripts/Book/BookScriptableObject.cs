using System.Collections;
using System.Collections.Generic;
using Anomaly;
using Sirenix.OdinInspector;
using UnityEngine;

#if true
using UnityEditor;
using UnityEngine.Windows;

#endif

namespace Book
{
    [CreateAssetMenu(menuName = "ScriptableObjects/BookData", fileName = "BookData")]
    public class BookScriptableObject : SerializedScriptableObject
    {
        public SortedDictionary<AnomalyType, List<PageScriptableObject>> pages;

#if UNITY_EDITOR
        // [Button]
        public void MakeFile()
        {
            string inputFolderPath = "Assets/Data/CH4_Anomaly";
            string outputFolderPath = "Assets/Data/Book/CH4";
                
            // 입력 폴더가 존재하지 않으면 생성
            if (!Directory.Exists(inputFolderPath))
            {
                Directory.CreateDirectory(inputFolderPath);
            }

            // 출력 폴더가 존재하지 않으면 생성
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            // 입력 폴더에서 모든 InputScriptableObject 에셋을 찾음
            string[] guids = AssetDatabase.FindAssets("t:AnomalyScriptableObject", new[] { inputFolderPath });
            List<AnomalyScriptableObject> inputObjects = new List<AnomalyScriptableObject>();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                AnomalyScriptableObject inputObject = AssetDatabase.LoadAssetAtPath<AnomalyScriptableObject>(assetPath);
                if (inputObject != null)
                {
                    inputObjects.Add(inputObject);
                }
            }

            // 각 입력 오브젝트에 대해 출력 오브젝트 생성
            foreach (var inputObject in inputObjects)
            {
                // 새로운 OutputScriptableObject 생성
                var outputObject = ScriptableObject.CreateInstance<PageScriptableObject>();
                outputObject.anomalyType = inputObject.type;
                outputObject.anomalyData = inputObject;

                // 에셋 저장
                string outputPath = $"{outputFolderPath}/page_{inputObject.name}.asset";
                AssetDatabase.CreateAsset(outputObject, outputPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
            Debug.Log($"처리 완료: {inputObjects.Count}개의 ScriptableObject 변환됨");
        }

        [Button]
        public void MakeDictionary()
        {
            pages = new SortedDictionary<AnomalyType, List<PageScriptableObject>>();
            string inputFolderPath = "Assets/Data/Book";
            
            // 입력 폴더에서 모든 InputScriptableObject 에셋을 찾음
            string[] guids = AssetDatabase.FindAssets("t:PageScriptableObject", new[] { inputFolderPath });
            
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                PageScriptableObject inputObject = AssetDatabase.LoadAssetAtPath<PageScriptableObject>(assetPath);
                if (inputObject != null)
                {
                    if (pages.ContainsKey(inputObject.anomalyType))
                    {
                        // 키 있음
                        pages[inputObject.anomalyType].Add(inputObject);
                    }
                    else
                    {
                        // 키 없음
                        pages.Add(inputObject.anomalyType, new List<PageScriptableObject>());
                        pages[inputObject.anomalyType].Add(inputObject);
                    }
                }
            }
        }
#endif
        
    }
}