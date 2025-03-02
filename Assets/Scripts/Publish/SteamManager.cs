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
        
        private void Start()
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
        }

        private void OnDestroy()
        {
            SteamClient.Shutdown();
        }
    }
}