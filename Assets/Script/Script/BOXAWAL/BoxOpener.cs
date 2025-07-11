using UnityEngine;
using System.Collections;

public class BoxOpener : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    //[Header("Semua HP di dalam kotak")]
    //public GameObject[] hpInsideBox; // Array untuk 3 HP

    //[Header("Delay waktu muncul HP setelah buka kotak")]
    //public float delayBeforeShowHP = 1.5f;

    [Header("Audio")]
    public AudioSource openBoxAudio; // 🔊 drag AudioSource di Inspector

    [Header("Panel Tutorial Setelah Buka Kardus")]
    public GameObject panelTutorial; // Drag panel GameObject di sini
    //public Animator tutorialAnimator; // Drag Animator dari panel
    //public string tutorialAnimName = "PanelSlideIn"; // Nama animasi di Animator
    //public float panelDisplayDuration = 3f; // Berapa detik panel muncul

    void Start()
    {
        animator = GetComponent<Animator>();

        // Matikan semua HP di awal
    //    foreach (GameObject hp in hpInsideBox)
    //    {
    //        if (hp != null)
    //            hp.SetActive(false);
    //    }

        // Panel tutorial dimatikan dulu
        if (panelTutorial != null)
            panelTutorial.SetActive(false);
    }

    void OnMouseDown()
    {
        if (!isOpen)
        {
            animator.Play("OpenBox");
            isOpen = true;

            // 🔊 Putar suara buka kardus
            if (openBoxAudio != null)
                openBoxAudio.Play();
            else
                Debug.LogWarning("❗ AudioSource belum diset di Inspector!");

            if (panelTutorial != null)
            {
                panelTutorial.SetActive(true);
            }

            //    StartCoroutine(ShowAllHPAfterDelay(delayBeforeShowHP));
        }
    }

    //IEnumerator ShowAllHPAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //
    //    foreach (GameObject hp in hpInsideBox)
    //    {
    //        if (hp != null)
    //            hp.SetActive(true);
    //    }
    //
        // Munculkan panel tutorial setelah buka kardus
    //    if (panelTutorial != null) //&& tutorialAnimator != null)
    //    {
    //        panelTutorial.SetActive(true);
        //    tutorialAnimator.Play("SlideIn", 0, 0f);
        //    Invoke(nameof(HidePanelTutorial), panelDisplayDuration);
    //    }
    //}

    void HidePanelTutorial()
    {
        if (panelTutorial != null)
            panelTutorial.SetActive(false);
    }
}
