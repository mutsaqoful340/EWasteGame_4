using UnityEngine;
using System.Collections;

public class BoxOpener : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private Camera cam;

    //[Header("Semua HP di dalam kotak")]
    //public GameObject[] hpInsideBox; // Array untuk 3 HP

    //[Header("Delay waktu muncul HP setelah buka kotak")]
    //public float delayBeforeShowHP = 1.5f;

    [Header("Audio")]
    public AudioSource openBoxAudio; // 🔊 drag AudioSource di Inspector

    [Header("Box Content")]
    public GameObject boxContent; // Drag panel GameObject di sini
    
    //public Animator tutorialAnimator; // Drag Animator dari panel
    //public string tutorialAnimName = "PanelSlideIn"; // Nama animasi di Animator
    //public float panelDisplayDuration = 3f; // Berapa detik panel muncul

    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        // Matikan semua HP di awal
        //    foreach (GameObject hp in hpInsideBox)
        //    {
        //        if (hp != null)
        //            hp.SetActive(false);
        //    }

        // Panel tutorial dimatikan dulu
        if (boxContent != null)
        {
            boxContent.SetActive(false);           
        }

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    openTheBox();
                }
            }
        }
    }

    void openTheBox()
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

            if (boxContent != null)
            {
                boxContent.SetActive(true);
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
        if (boxContent != null)
            boxContent.SetActive(false);
    }
}