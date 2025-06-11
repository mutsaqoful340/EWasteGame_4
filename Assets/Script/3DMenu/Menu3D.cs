using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Menu3D : MonoBehaviour
{
    Audio_Manager audioManager;
    private Animator animator; // Reference to Animator

    private bool isShowing = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<Audio_Manager>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (isShowing)
        //    {
        //        animator.Play("GP_LaptopIN");
        //    }
        //    else
        //    {
        //        animator.Play("GP_LaptopOUT");
        //    }
        //
        //    isShowing = !isShowing;
        //}
    }

    public void PlayCameraAnimation(string triggerName)
    {
        if (animator != null)
        {
            audioManager.PlaySFX(audioManager.buttonClick);
            animator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning("No Animator found on the Camera!");
        }
    }

    public void PlayAnimName(string animationName)
    {
        if (animator != null)
        {
            audioManager.PlaySFX(audioManager.buttonClick);
            animator.Play(animationName);
        }
        else
        {
            Debug.LogWarning("No Animator found on the Camera!");
        }
    }

    // Call this to show the canvas
    public void ActivateCanvas(GameObject canvasToShow)
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.buttonClick);

        if (canvasToShow != null)
        {
            canvasToShow.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No Canvas assigned to activate.");
        }
    }

    public void DeactivateCanvas(GameObject canvasToHide)
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.buttonClick);

        if (canvasToHide != null)
        {
            canvasToHide.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No Canvas assigned to deactivate.");
        }
    }

    public void TS_Laptop()
    {

    }
}