using UnityEngine;



public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialSteps;
    public int currentStep = 0;

    void Start()
    {
        Time.timeScale = 0f;
        ShowStep(currentStep);
    }

    public void NextStep()
    {
        tutorialSteps[currentStep].SetActive(false);
        currentStep++;

        if (currentStep < tutorialSteps.Length)
        {
            ShowStep(currentStep);
        }
        else
        {
            Time.timeScale = 1f; // Resume game
            this.gameObject.SetActive(false); // Sembunyikan UI tutorial
        }
    }

    void ShowStep(int step)
    {
        tutorialSteps[step].SetActive(true);
    }
}
