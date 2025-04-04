using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;

public class AnomalyClearData
{
    public Dictionary<string, bool> anomalyClearDictionary; // 이상현상 이름, 클리어 여부.

    public int clearCount = 0;

    public int[] dict_by_chapter = new int[4];

    public void AddData(string anomalyName)
    {
        int t = anomalyName[0] - '1';
        dict_by_chapter[t]++;
        anomalyClearDictionary.Add(anomalyName, false);  
        clearCount++;
    }
    
    public bool TryCheckData(string anomalyName)
    {
        if (anomalyClearDictionary.ContainsKey(anomalyName))
        {
            int t = anomalyName[0] - '1';
            dict_by_chapter[t]--;

            if (dict_by_chapter[t] == 0 && SteamClient.IsValid)
            {
                Achievement ach;
                switch (t)
                {
                    case 0:
                        ach = new Achievement("DIARY1CH");
                        break;
                    case 1:
                        ach = new Achievement("DIARY2CH");
                        break;
                    case 2:
                        ach = new Achievement("DIARY3CH");
                        break;
                    case 3:
                        ach = new Achievement("DIARY4CH");
                        break;
                }
                if (!ach.State)
                    ach.Trigger();
            }

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