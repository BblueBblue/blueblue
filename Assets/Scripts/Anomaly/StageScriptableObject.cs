using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Anomaly
{
    [CreateAssetMenu(menuName = "ScriptableObjects/StageData")]
    public class StageScriptableObject : ScriptableObject
    {
        public string stageName;
        [EnumToggleButtons, HideLabel]
        public AnomalyType type;
        [AssetsOnly]
        public AssetReference defaultReference;
        public List<AnomalyScriptableObject> problems;
        
#if UNITY_EDITOR
        [Button]
        public void MakeProblemsList()
        {
            problems = new List<AnomalyScriptableObject>();
            string[] guids = AssetDatabase.FindAssets("t:AnomalyScriptableObject");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AnomalyScriptableObject problemData = AssetDatabase.LoadAssetAtPath<AnomalyScriptableObject>(path);
                if (problemData.type.Equals(this.type))
                {
                    problems.Add(problemData);
                }
            }
        }
#endif
    }
}