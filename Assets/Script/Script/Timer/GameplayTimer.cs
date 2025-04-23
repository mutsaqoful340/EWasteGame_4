using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Pindah ke atas

public class GameplayTimer : MonoBehaviour
{
    public float waktuTersisa = 300f; // 5 menit waktu permainan
    private float waktuMulai;
    public bool permainanSelesai = false;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI upahText;
    public TextMeshProUGUI winText;
    public GameObject winPanel;
    public GameObject buttonsPanel;

    void Start()
    {
        waktuMulai = Time.time;
        winPanel.SetActive(false);
        buttonsPanel.SetActive(false);
    }

    void Update()
    {
        if (!permainanSelesai)
        {
            waktuTersisa -= Time.deltaTime;
            UpdateTimerUI();

            if (waktuTersisa <= 0f)
            {
                waktuTersisa = 0f;
                SelesaikanPermainan();
            }
        }
    }

    void UpdateTimerUI()
    {
        float menit = Mathf.Floor(waktuTersisa / 60);
        float detik = Mathf.Floor(waktuTersisa % 60);
        timerText.text = string.Format("{0:00}:{1:00}", menit, detik);
    }

    public void SelesaikanPermainan()
    {
        if (permainanSelesai) return;

        permainanSelesai = true;
        Debug.Log("Permainan selesai");

        float durasiMain = Time.time - waktuMulai;
        int upah = HitungUpah(durasiMain);

        upahText.text = "Upah: Rp " + upah.ToString("N0");
        winText.text = "Anda Menang!";
        winPanel.SetActive(true);

        StartCoroutine(TampilkanTombol());
    }

    int HitungUpah(float durasi)
    {
        if (durasi <= 60f) return 25000;
        else if (durasi <= 120f) return 20000;
        else if (durasi <= 180f) return 15000;
        else if (durasi <= 240f) return 10000;
        else if (durasi <= 300f) return 5000;
        else return 0;
    }

    IEnumerator TampilkanTombol()
    {
        yield return new WaitForSeconds(1f);
        buttonsPanel.SetActive(true);
    }

    public void LanjutLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Tidak ada level selanjutnya.");
        }
    }

    public void UlangiLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
