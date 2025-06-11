using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialSteps;
    private int currentStep = 0;

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
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }

    void ShowStep(int step)
    {
        GameObject stepPanel = tutorialSteps[step];
        stepPanel.SetActive(true);
        CanvasGroup cg = stepPanel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0;
            StartCoroutine(FadeIn(cg));
        }
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float duration = 0.5f;
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }
}
