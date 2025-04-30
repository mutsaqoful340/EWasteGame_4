using System.Collections;
using UnityEngine;

public class BackgroundTransition : MonoBehaviour
{
    public CanvasGroup background1;
    public CanvasGroup background2;
    public float transitionDuration = 1f; // Durasi transisi

    void Start()
    {
        StartCoroutine(FadeBackgrounds());
    }

    private IEnumerator FadeBackgrounds()
    {
        float timer = 0f;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / transitionDuration;
            background1.alpha = 1 - alpha;
            background2.alpha = alpha;
            yield return null;
        }

        // Pastikan alpha pas di akhir
        background1.alpha = 0;
        background2.alpha = 1;
    }
}
