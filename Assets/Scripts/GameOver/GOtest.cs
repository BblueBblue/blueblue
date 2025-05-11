using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameover {
    public class GOtest : MonoBehaviour
    {
        public GameOver go;

        public bool isTrigger = false;

        private void Start()
        {
            go = GetComponent<GameOver>();
        }
        private void Update()
        {
            if(isTrigger)
                go.StartGameOver();
        }
    } 
}
