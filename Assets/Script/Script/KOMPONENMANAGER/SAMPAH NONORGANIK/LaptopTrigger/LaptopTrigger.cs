using UnityEngine;
using System.Collections.Generic;

public class LaptopTutorialTrigger : MonoBehaviour
{
    private static HashSet<string> clickedLaptopIDs = new HashSet<string>();

    public string laptopID;
    public GameObject panelObject;
    public Animator tutorialAnimator;
    public string animationName = "LaptopTutorial_SlideIn";
    public float displayDuration = 3f;

    private bool tutorialActive = false;

    void OnMouseDown()
    {
        if (!clickedLaptopIDs.Contains(laptopID))
        {
            clickedLaptopIDs.Add(laptopID);
            ShowTutorial();
        }
    }

    void ShowTutorial()
    {
        if (panelObject == null || tutorialAnimator == null) return;

        panelObject.SetActive(true);
        tutorialAnimator.Play(animationName, 0, 0f);
        tutorialActive = true;

        Invoke("HidePanel", displayDuration);
    }

    void HidePanel()
    {
        if (tutorialActive)
        {
            panelObject.SetActive(false);
            tutorialActive = false;
        }
    }

    private void OnDestroy()
    {
        if (tutorialActive && panelObject != null)
        {
            panelObject.SetActive(false);
        }
    }
}
