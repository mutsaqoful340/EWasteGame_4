using UnityEngine;
using System.Collections;

public class BoxOpener : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    [Header("Semua HP di dalam kotak")]
    public GameObject[] hpInsideBox; // Array untuk 3 HP

    [Header("Delay waktu muncul HP setelah buka kotak")]
    public float delayBeforeShowHP = 1.5f;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Matikan semua HP di awal
        foreach (GameObject hp in hpInsideBox)
        {
            if (hp != null)
                hp.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (!isOpen)
        {
            animator.Play("OpenBox");
            isOpen = true;

            StartCoroutine(ShowAllHPAfterDelay(delayBeforeShowHP));
        }
    }

    IEnumerator ShowAllHPAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject hp in hpInsideBox)
        {
            if (hp != null)
                hp.SetActive(true);
        }
    }
}
