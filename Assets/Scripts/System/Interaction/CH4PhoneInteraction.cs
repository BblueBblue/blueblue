using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Interaction
{
    public class CH4PhoneInteraction : MonoBehaviour, IInteractable
    {
        public AudioClip daughter;
        public AudioClip wife;
        public AudioClip beep;

        private float daugterTime = 0f;
        private float wifeTime = 0f;

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
            wifeTime = wife.length;
            SoundManager.Instance.PlaySFX(sfxPlayer,beep,true);
            phonecallSurface.SetActive(true);
        }

        IEnumerator PhoneCall()
        {
            SoundManager.Instance.PlaySFX(sfxPlayer, daughter, false);
            yield return new WaitForSeconds(daugterTime);
            SoundManager.Instance.PlaySFX(sfxPlayer, wife, false);
            yield return new WaitForSeconds(wifeTime + 0.2f);
            phoneNormalSurface.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            blackSurface.SetActive(true);
        }
    }
}