using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class VNDialog
{
    public string namaKarakter;
    [TextArea(2, 5)]
    public string dialog;
}

public class VNDialogManager : MonoBehaviour
{
    [Header("UI Komponen")]
    public GameObject vnPanel;
    public TextMeshProUGUI namaText;
    public TextMeshProUGUI dialogText;
    public Button btnLanjut;

    [Header("Daftar Dialog (Isi dari Inspector)")]
    public List<VNDialog> dialogList;

    [Header("Audio")]
    public AudioSource typingAudio;

    [Header("Apakah Ini VN Akhir?")]
    public bool isVNEnding = false;

    [Header("Efek Karakter")]
    public GameObject karakterObjek;
    public float scaleAmount = 1.05f;
    public float scaleSpeed = 2f;

    [Header("Panel Tutorial Setelah VN")]
    public GameObject panelTutorial;
    public Animator tutorialAnimator;
    public string tutorialAnimName = "LaptopTutorial_SlideIn";
    public float tutorialDisplayDuration = 3f;

    public int dialogIndex = 0;

    private bool isTyping = false;
    private bool selesaiVN = false;
    private Coroutine typingCoroutine;
    private Coroutine pulseCoroutine;
    private Vector3 karakterOriginalScale;

    void Start()
    {
        if (dialogList != null && dialogList.Count > 0)
        {
            StartVN();
        }
        else
        {
            vnPanel.SetActive(false);
        }
    }

    public void StartVN(List<VNDialog> dialogBaru = null)
    {
        if (dialogBaru != null)
            dialogList = dialogBaru;

        dialogIndex = 0;
        selesaiVN = false;

        vnPanel.SetActive(true);
        Time.timeScale = 0f;

        if (karakterObjek != null)
        {
            karakterOriginalScale = karakterObjek.transform.localScale;
            pulseCoroutine = StartCoroutine(PulseKarakterObject());
        }

        if (btnLanjut != null)
        {
            btnLanjut.onClick.RemoveAllListeners();
            btnLanjut.onClick.AddListener(NextDialog);
        }

        ShowDialog(dialogIndex);
    }

    public void ShowDialog(int index)
    {
        if (index < dialogList.Count)
        {
            namaText.text = dialogList[index].namaKarakter;

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeText(dialogList[index].dialog));
        }
        else
        {
            SelesaiVisualNovel();
        }
    }

    void NextDialog()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogText.text = dialogList[dialogIndex].dialog;
            isTyping = false;

            if (typingAudio != null && typingAudio.isPlaying)
                typingAudio.Stop();
        }
        else
        {
            dialogIndex++;
            ShowDialog(dialogIndex);
        }
    }

    IEnumerator TypeText(string text)
    {
        dialogText.text = "";
        isTyping = true;

        if (typingAudio != null)
            typingAudio.Play();

        foreach (char c in text)
        {
            dialogText.text += c;
            yield return new WaitForSecondsRealtime(0.03f);
        }

        if (typingAudio != null && typingAudio.isPlaying)
            typingAudio.Stop();

        isTyping = false;
    }

    void SelesaiVisualNovel()
    {
        selesaiVN = true;

        vnPanel.SetActive(false);
        Time.timeScale = 1f;

        if (typingAudio != null && typingAudio.isPlaying)
            typingAudio.Stop();

        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        if (karakterObjek != null)
            karakterObjek.transform.localScale = karakterOriginalScale;

        Debug.Log("📢 VN Selesai");

        if (isVNEnding)
        {
            Debug.Log("📢 VN akhir terdeteksi, menampilkan ringkasan...");

            WasteZone ewasteZone = FindObjectOfType<WasteZone>();
            if (ewasteZone != null)
            {
                ewasteZone.TampilkanRingkasanLangsungDariVN();
                return;
            }

            BoxPenyimpanan penyimpanan = FindObjectOfType<BoxPenyimpanan>();
            if (penyimpanan != null)
            {
                gameObject.SetActive(false);
                return;
            }

            Debug.LogWarning("❗ WasteZone dan BoxPenyimpanan tidak ditemukan.");
        }
        else
        {
            // VN awal selesai, tampilkan panel tutorial
            ShowTutorialSetelahVN();
        }
    }

    void ShowTutorialSetelahVN()
    {
        Debug.Log("👉 Memanggil panel tutorial setelah VN.");

        if (panelTutorial == null || tutorialAnimator == null)
        {
            Debug.LogWarning("⚠️ Panel Tutorial atau Animator belum diset.");
            return;
        }

        panelTutorial.SetActive(true);
        tutorialAnimator.Play("SlideIn", 0, 0f);
        Invoke(nameof(HideTutorialPanel), tutorialDisplayDuration);
    }

    void HideTutorialPanel()
    {
        if (panelTutorial != null)
        {
            panelTutorial.SetActive(false);
        }
    }

    IEnumerator PulseKarakterObject()
    {
        if (karakterObjek == null) yield break;

        Vector3 targetScale = karakterOriginalScale * scaleAmount;

        while (!selesaiVN)
        {
            float t = 0;
            while (t < 1f && !selesaiVN)
            {
                t += Time.unscaledDeltaTime * scaleSpeed;
                karakterObjek.transform.localScale = Vector3.Lerp(karakterOriginalScale, targetScale, t);
                yield return null;
            }

            t = 0;
            while (t < 1f && !selesaiVN)
            {
                t += Time.unscaledDeltaTime * scaleSpeed;
                karakterObjek.transform.localScale = Vector3.Lerp(targetScale, karakterOriginalScale, t);
                yield return null;
            }
        }
    }
}
