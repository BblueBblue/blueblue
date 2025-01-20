using System.Collections;
using System.Collections.Generic;
using Anomaly;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Book
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PageData", fileName = "PageData")]
    public class PageScriptableObject : ScriptableObject
    {
        public AnomalyType anomalyType;
        public AnomalyScriptableObject anomalyData;
        [Title("Photo")]
        [HideLabel]
        [PreviewField(256)]
        public Sprite photo;
        [Title("Description")]
        [HideLabel]
        [MultiLineProperty]
        public string description;
    }
}
