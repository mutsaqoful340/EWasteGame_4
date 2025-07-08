using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPlayer : MonoBehaviour
{
    [Header("UI Tutorial")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    [Header("Pointer Panah")]
    public GameObject arrowPointerPrefab;
    private GameObject currentArrow;

    [Header("Target Objek Tutorial")]
    public GameObject cardboardBox; // kotak
    public GameObject firstItem;    // item pertama
    public GameObject binTarget;    // tempat buang

    private int step = 0;
    private bool tutorialActive = false;

    void Start()
    {

    }

    public void StartTutorial()
    {
        tutorialPanel.SetActive(true);
        step = 0;
        tutorialActive = true;
        ShowStep(0);
    }

    public void ShowStep(int nextStep)
    {
        if (!tutorialActive) return;

        // Hapus panah lama jika ada
        if (currentArrow != null)
            Destroy(currentArrow);

        step = nextStep;

        switch (step)
        {
            case 0:
                tutorialText.text = "Selamat datang! Yuk kita pelajari cara mainnya.";
                break;

            case 1:
                tutorialText.text = "Klik kardus untuk membukanya.";
                currentArrow = SpawnArrow(cardboardBox);
                break;

            case 2:
                tutorialText.text = "Klik barang di dalam kardus untuk mengeluarkannya.";
                currentArrow = SpawnArrow(firstItem);
                break;

            case 3:
                tutorialText.text = "Seret barang ke tempat sampah yang sesuai.";
                currentArrow = SpawnArrow(binTarget);
                break;

            case 4:
                tutorialText.text = "Bagus! Kamu sudah siap bermain!";
                tutorialActive = false;
                Invoke(nameof(HideTutorial), 2f);
                break;
        }
    }

    GameObject SpawnArrow(GameObject target)
    {
        if (target == null) return null;

        GameObject arrow = Instantiate(arrowPointerPrefab);
        ArrowFollowTarget follow = arrow.GetComponent<ArrowFollowTarget>();
        if (follow != null)
        {
            follow.target = target.transform;
        }

        return arrow;
    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false);

        if (currentArrow != null)
            Destroy(currentArrow);
    }

    // Opsional, jika masih mau pakai tombol manual
    public void NextStepButton()
    {
        ShowStep(step + 1);
    }
}
