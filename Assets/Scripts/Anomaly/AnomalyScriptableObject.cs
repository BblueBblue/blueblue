using System.Collections;
using System.Collections.Generic;
using Anomaly;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Anomaly
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ProblemData", fileName = "ProblemData")]
    public class AnomalyScriptableObject : ScriptableObject
    {
        [EnumToggleButtons, HideLabel]
        public AnomalyType type;
        public string anomalyName;
        [AssetsOnly]
        public AssetReference anomalyReference;
    }
}