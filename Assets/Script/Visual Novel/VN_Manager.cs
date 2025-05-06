using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VN_Manager : MonoBehaviour
{
    Audio_Manager audioManager;

    private Animator animator; // Reference to Animator

    void Start()
    {
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<Audio_Manager>();
    }

    void Update()
    {

    }

    // These two has the same function, it's just for management thing
    public void PlayDialAnim(string triggerName)
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

    public void Play_VNAnim(string triggerName)
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

    // Call this to show the canvas
    public void ON_VNCanvas(GameObject canvasToShow)
    {
        if (canvasToShow != null)
        {
            canvasToShow.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No Canvas assigned to activate.");
        }
    }

    // Call this to hide the canvas
    public void OFF_VNCanvas(GameObject canvasToHide)
    {
        if (canvasToHide != null)
        {
            canvasToHide.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No Canvas assigned to deactivate.");
        }
    }

    public void ON_VNDialogue(GameObject canvasToShow)
    {
        if (canvasToShow != null)
        {
            canvasToShow.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No Dialogue assigned to activate.");
        }
    }

    // Call this to hide the canvas
    public void OFF_VNDialogue(GameObject canvasToHide)
    {
        if (canvasToHide != null)
        {
            canvasToHide.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No Dialogue assigned to deactivate.");
        }
    }
}
