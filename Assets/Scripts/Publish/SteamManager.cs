using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

namespace publish
{
    public class SteamManager : MonoBehaviour
    {
        [SerializeField]
        private uint steam_app_id;
        
        private void Awake()
        {
            try
            {
                SteamClient.Init(steam_app_id);
                Debug.Log("Access");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Access Failure");
            }
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            SteamClient.Shutdown();
        }
        private void OnApplicationQuit()
        {
            try
            {
                SteamClient.Shutdown();
            }
            catch (System.Exception e)
            {

            }
        }
    }
}