using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("Pertanyaan & Jawaban")]
    public TextMeshProUGUI pertanyaanText;
    public Button[] pilihanButtons; // Assign BtnA, BtnB, BtnC secara urut
    public int indexJawabanBenar = 1; // Misalnya: jawaban ke-2 adalah yang benar

    [Header("Feedback Section")]
    public GameObject feedBackSession;
    public Image iconKarakter;
    public Sprite iconHappy;
    public Sprite iconSad;
    public TextMeshProUGUI feedbackText;
    public Button tombolNext;

    [Header("SFX (Optional)")]
    public AudioSource audioSource;
    public AudioClip sfxBenar;
    public AudioClip sfxSalah;

    private bool sudahDijawab = false;

    void Start()
    {
        feedBackSession.SetActive(false);
        for (int i = 0; i < pilihanButtons.Length; i++)
        {
            int index = i; // lokal copy untuk closure
            pilihanButtons[i].onClick.AddListener(() => Jawab(index));
        }

        tombolNext.onClick.AddListener(() =>
        {
            feedBackSession.SetActive(false);
            // Tambahkan logika lanjut level atau tutup quiz di sini
            Debug.Log("Lanjut ke langkah berikutnya...");
        });
    }

    public void Jawab(int index)
    {
        if (sudahDijawab) return;
        sudahDijawab = true;

        // Disable semua tombol
        foreach (Button btn in pilihanButtons)
            btn.interactable = false;

        bool benar = index == indexJawabanBenar;

        // Umpan balik visual
        feedBackSession.SetActive(true);
        iconKarakter.sprite = benar ? iconHappy : iconSad;
        feedbackText.text = benar
            ? "✅ Jawabanmu benar! Baterai memang berbahaya."
            : "❌ Salah. Baterai bisa mencemari lingkungan, loh.";

        // Mainkan suara
        if (audioSource != null)
        {
            audioSource.clip = benar ? sfxBenar : sfxSalah;
            audioSource.Play();
        }
    }
}
