using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Anim_Manager : MonoBehaviour
    {
        Audio_Manager audioManager;

        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio_Manager>(); //To awake the "AudioManager" game object when the game started.

        }

        public GameObject popPCB, popMenuButtons;
        private Animator animator1, animator2;
        //animator1 > popup PCB
        //animator2 > popup MenuButtons

        private bool isPaused = false; // track menu state

        private void Start()
        {
            animator1 = popPCB.GetComponent<Animator>();
            animator2 = popMenuButtons.GetComponent<Animator>();
        }
        
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // To open Pause Menu popup
            {
                if (isPaused)
                {
                   HidePopup();
                }
                else
                {
                    ShowPopup();
                }

                isPaused = !isPaused;
            }
        }

        public void ShowPopup() // To show popup
        {
            audioManager.PlaySFX(audioManager.buttonClick);
            animator1.Play("PCB_IN");
            animator2.Play("BTN_IN");
        }

        public void HidePopup() // To hide popup
        {
            audioManager.PlaySFX(audioManager.buttonClick);
            animator1.Play("PCB_OUT");
            animator2.Play("BTN_OUT");
        }
    }
