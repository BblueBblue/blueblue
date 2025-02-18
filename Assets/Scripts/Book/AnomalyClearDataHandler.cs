using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Book;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class AnomalyClearDataHandler : MonoBehaviour
{
    [SerializeField] 
    private BookScriptableObject bookData;
    
    private AnomalyClearData anomalyData;
    private string savePath;

    public UnityEvent<AnomalyClearData> onLoadData;
    public UnityEvent<string> onAnomalyChecked;
    
#if UNITY_EDITOR
    public bool forceUpdate = true;
#endif

    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        Debug.Log(savePath);
        LoadData();
    }
    
    private void LoadData()
    {
#if UNITY_EDITOR
        if (forceUpdate)
        {
            anomalyData = MakeNewData();
            onLoadData.Invoke(anomalyData);
            return;
        }
#endif
        try
        {
            if (File.Exists(savePath))
            {
                // 파일에서 JSON 데이터 읽기
                string jsonData = File.ReadAllText(savePath);
                
                // JSON 문자열을 SaveData 객체로 변환
                anomalyData = JsonConvert.DeserializeObject<AnomalyClearData>(jsonData);
                Debug.Log("게임 로드 완료");
            }
            else
            {
                Debug.Log("세이브 파일이 없어 새로운 데이터를 생성합니다.");
                anomalyData = MakeNewData();
            }

            onLoadData.Invoke(anomalyData);
        }
        catch (Exception e)
        {
            Debug.LogError($"로드 중 에러 발생: {e.Message}");
            anomalyData = MakeNewData();
        }
    }

    private void SaveData()
    {
        try 
        {
            // Dictionary를 포함한 데이터를 JSON 문자열로 변환
            string jsonData = JsonConvert.SerializeObject(anomalyData);
            
            // 파일에 JSON 데이터 쓰기
            File.WriteAllText(savePath, jsonData);
            Debug.Log($"게임 저장 완료: {savePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"저장 중 에러 발생: {e.Message}");
        }
    }

    private AnomalyClearData MakeNewData()
    {
        AnomalyClearData data = new AnomalyClearData();
        foreach (var pagesData in bookData.pages)
        {
            foreach (var pageData in pagesData.Value)
            {
                data.AddData(pageData.anomalyData.anomalyName);
            }
        }

        return data;
    }

    private void OnDestroy()
    {
        SaveData();
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void CheckAnomaly(string anomalyName)
    {
        if (anomalyData.TryCheckData(anomalyName))
        {
            // 체크 성공
            onAnomalyChecked.Invoke(anomalyName);
        }
    }
}
