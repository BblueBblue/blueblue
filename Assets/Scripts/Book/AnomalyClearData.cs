using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;

public class AnomalyClearData
{
    public Dictionary<string, bool> anomalyClearDictionary; // 이상현상 이름, 클리어 여부.

    public int clearCount = 0;

    public void AddData(string anomalyName)
    {
        anomalyClearDictionary.Add(anomalyName, false);  
        clearCount++;
    }
    
    public bool TryCheckData(string anomalyName)
    {
        if (anomalyClearDictionary.ContainsKey(anomalyName))
        {
            anomalyClearDictionary[anomalyName] = true;
            clearCount--;

            if(clearCount == 0 && SteamClient.IsValid)
            {
                Achievement ach = new Achievement("DIARYCLEAR");

                if (!ach.State)
                    ach.Trigger();
            }
            return true;
        }

        return false;
    }
    
    public AnomalyClearData()
    {
        anomalyClearDictionary = new Dictionary<string, bool>();
    }
}