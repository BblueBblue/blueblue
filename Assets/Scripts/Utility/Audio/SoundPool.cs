using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundPool
{
    public string ClipName { get { return audioClip.name; } }
    public AudioClip audioClip;
    [SerializeField]
    protected List<AudioPlayer> audioPlayerList = new List<AudioPlayer>();
    public int maxPlayCount = 5;

    public void AddAudioPlayer(AudioPlayer audioPlayer)
    {
        if (audioPlayerList.Contains(audioPlayer)) { 
            return;
        }
        audioPlayer.ChangeMasterVolume(SoundManager.Instance.SFXMasterVolume);
        audioPlayerList.Add(audioPlayer);
    }

    public void RemoveAudioPlayer(AudioPlayer audioPlayer)
    {
        audioPlayerList.Remove(audioPlayer);
    }

    public void ChangeMasterVolume(float masterVolume)
    {
        audioPlayerList.RemoveAll(audioPlayer => audioPlayer == null || audioPlayer.AudioSource == null);
        
        for (var i = 0; i < audioPlayerList.Count; ++i)
        {
            audioPlayerList[i].ChangeMasterVolume(masterVolume);
        }
    }

    public void CheckPool()
    {
        AudioPlayer audioPlayer = null;
        //5개 이상 재생인 경우 Stop후에 재생합니다..
        if (maxPlayCount <= audioPlayerList.Count)
        {
            var minTime = float.MaxValue;

            for (var i = 0; i < audioPlayerList.Count; ++i)
            {
                if (audioPlayerList[i].Time < minTime)
                {
                    audioPlayer = audioPlayerList[i];
                    minTime = audioPlayer.Time;
                }
            }

            audioPlayer?.Stop();
            audioPlayerList.RemoveAt(0);
        }
    }

}
