using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Interaction
{
    public class CH4PhoneInteraction : MonoBehaviour, IInteractable
    {
        public AudioClip daughter;
        public AudioClip talk;
        public AudioClip beep;
        public AudioClip connect;

        private float daugterTime = 0f;
        private float talkTime = 0f;
        private float connectTime = 0f;
        private bool isTriggered = false;

        public GameObject blackSurface;
        public GameObject phonecallSurface;
        public GameObject phoneNormalSurface;

        public SFXPlayer sfxPlayer;
        public void StartInteract()
        {
            StartCoroutine(PhoneCall());
        }

        public void StopInteract()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            daugterTime = daughter.length;
            talkTime = talk.length;
            connectTime = connect.length;
            SoundManager.Instance.PlaySFX(sfxPlayer,beep,true);
            phonecallSurface.SetActive(true);
        }

        IEnumerator PhoneCall()
        {
            if (!isTriggered)
            {
                isTriggered = true;
                SoundManager.Instance.PlaySFX(sfxPlayer, connect, false);
                yield return new WaitForSeconds(connectTime);
                //SoundManager.Instance.PlaySFX(sfxPlayer, daughter, false);
                //yield return new WaitForSeconds(daugterTime);
                SoundManager.Instance.PlaySFX(sfxPlayer, talk, false);
                yield return new WaitForSeconds(talkTime + 0.2f);
                phoneNormalSurface.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                blackSurface.SetActive(true);
            }
            
        }
    }
}