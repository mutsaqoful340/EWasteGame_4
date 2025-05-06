using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BoxPenyimpanan : MonoBehaviour
{
    [Header("Peringatan & Ending")]
    public GameObject overlayPeringatan;
    public GameObject efekBuram;
    public GameObject endingPanel;

    private bool sudahMakanHariIni = false;
    private int hariTidakMakan = 0;

    public float totalTime = 300f;
    private float timer;

    public int maxItems = 3;
    private int currentItems = 0;

    public GameObject financeSummaryPanel;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI pemasukanText;
    public TextMeshProUGUI sisaUangText;

    public Toggle toggleMakan;
    public Toggle toggleNabung;
    public Toggle toggleJajan;

    public Button btnLihatRingkasan;
    public Button btnAkhiriGame;

    private bool isGameOver = false;

    [Header("Reward Settings")]
    public int initialReward = 50000; // Bisa diubah dari Inspector
    private int currentReward;

    private int lastMinuteChecked = -1;

    private int makanCost = 15000;
    private int nabungCost = 15000;
    private int jajanCost = 10000;



    private float delayToSummary = 2f;

    private bool buffJajanSudahDipakai = false;
    private bool jajanSudahDiterapkan = false;

    private bool overlaySudahTampil = false;


    void Start()
    {
        // Reset overlay flag saat masuk ke level baru
        if (SceneManager.GetActiveScene().name == "3DLV2")
        {
            overlaySudahTampil = false; // Reset flag agar overlay bisa muncul lagi jika diperlukan
        }

        timer = totalTime;

        // Cek apakah sudah ada buff jajan yang aktif
        if (PlayerPrefs.GetInt("BuffJajanAktif", 0) == 1)
        {
            timer += 300f;
            PlayerPrefs.SetInt("BuffJajanAktif", 0);
        }

        // Reset sisa uang hanya di level 1
        if (SceneManager.GetActiveScene().name == "3DLV1")
        {
            PlayerPrefs.DeleteKey("SisaUang");
            PlayerPrefs.SetInt("TidakMakanKemarin", 0); // Reset status makan
        }

        int sisaUangDariLevelSebelumnya = PlayerPrefs.GetInt("SisaUang", 0);
        currentReward = sisaUangDariLevelSebelumnya + initialReward;

        UpdateTimerUI();
        UpdateMoneyUI();

        // Pastikan hanya tampilkan overlay jika pemain belum makan hari sebelumnya dan belum pernah tampil
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "3DLV1" && PlayerPrefs.GetInt("TidakMakanKemarin", 0) == 1 && !sudahMakanHariIni)
        {
            if (overlayPeringatan != null)
            {
                overlayPeringatan.SetActive(true);
            }

            StartCoroutine(TungguDanLanjut());
        }
        else if (currentScene == "3DLV2")
        {
            // Di level 2, pastikan overlay tidak muncul
            if (overlayPeringatan != null)
            {
                overlayPeringatan.SetActive(false);
            }
        }
    }



    void Update()
    {
        if (isGameOver) return;

        timer -= Time.deltaTime;
        if (timer < 0f) timer = 0f;

        UpdateTimerUI();

        // Mengurangi reward setiap menit
        int minutesPassed = Mathf.FloorToInt((totalTime - timer) / 60f);
        if (minutesPassed > lastMinuteChecked)
        {
            lastMinuteChecked = minutesPassed;
            currentReward = Mathf.Max(0, currentReward - (minutesPassed * 10000));
            UpdateMoneyUI();
        }

        if (timer <= 0f)
        {
            GameOver();
        }
    }

    public void AddItem(string itemType)
    {
        if (isGameOver) return;

        currentItems++;
        Debug.Log($"Item ditambahkan: {currentItems}/{maxItems}");

        if (currentItems >= maxItems)
        {
            GameOver();
        }
    }

    public void RemoveItem()
    {
        currentItems--;
        Debug.Log($"Item dihancurkan. Item sekarang: {currentItems}/{maxItems}");
    }

    public bool IsFull()
    {
        return currentItems >= maxItems;
    }

    void GameOver()
    {
        isGameOver = true;

        // Jangan langsung mengubah reward menjadi 0
        if (timer <= 0f)
        {
            currentReward = 0;
        }

        // Tampilkan ringkasan keuangan hanya jika game sudah selesai
        ShowFinanceSummary();
    }

    void ShowFinanceSummary()
    {
        if (financeSummaryPanel != null && isGameOver)
        {
            financeSummaryPanel.SetActive(true); // Pastikan hanya muncul saat game over
        }

        if (pemasukanText != null)
            pemasukanText.text = "Pemasukan: Rp" + currentReward.ToString("N0");

        UpdateSisaUang();

        toggleMakan.onValueChanged.AddListener(delegate { UpdateSisaUang(); });
        toggleNabung.onValueChanged.AddListener(delegate { UpdateSisaUang(); });
        toggleJajan.onValueChanged.AddListener(delegate { UpdateSisaUang(); });
    }


    void UpdateSisaUang()
    {
        int totalPengeluaran = 0;

        if (toggleMakan.isOn) totalPengeluaran += makanCost;
        if (toggleNabung.isOn) totalPengeluaran += nabungCost;
        if (toggleJajan.isOn) totalPengeluaran += jajanCost;

        int sisa = currentReward - totalPengeluaran;
        sisa = Mathf.Max(0, sisa);

        if (sisaUangText != null)
            sisaUangText.text = "Sisa: Rp" + sisa.ToString("N0");

        PlayerPrefs.SetInt("SisaUang", sisa);
        PlayerPrefs.Save();
    }

    public void TerapkanPilihan()
    {
        if (jajanSudahDiterapkan) return;

        // Tentukan apakah player makan dulu
        bool playerMakan = toggleMakan.isOn;
        bool playerJajan = toggleJajan.isOn;

        // === BUFF JAJAN ===
        if (playerJajan && !buffJajanSudahDipakai && currentReward >= jajanCost)
        {
            currentReward -= jajanCost;
            timer += 300f;
            buffJajanSudahDipakai = true;
            PlayerPrefs.SetInt("BuffJajanAktif", 1);
        }

        // === CEK MAKAN ===
        if (playerMakan && currentReward >= makanCost)
        {
            currentReward -= makanCost;
            sudahMakanHariIni = true;
        }
        else
        {
            sudahMakanHariIni = false;
        }

        // Simpan status tidak makan untuk level berikutnya
        if (sudahMakanHariIni)
        {
            PlayerPrefs.SetInt("TidakMakanKemarin", 0);
        }
        else
        {
            PlayerPrefs.SetInt("TidakMakanKemarin", 1);
        }

        PlayerPrefs.Save(); // ⬅️ PENTING

        UpdateMoneyUI();
        UpdateTimerUI();

        // **Penting**: Hapus overlay jika sudah makan
        if (sudahMakanHariIni && overlayPeringatan != null)
        {
            overlayPeringatan.SetActive(false); // Menyembunyikan overlay jika pemain memilih makan
        }

        // Jika pemain tidak memilih makan, pastikan overlay tetap aktif
        if (!sudahMakanHariIni && overlayPeringatan != null && !overlaySudahTampil)
        {
            overlayPeringatan.SetActive(true); // Tampilkan overlay jika belum memilih makan
        }
    }




    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "Uang: Rp" + currentReward.ToString("N0");
    }



    IEnumerator DelayEnding()
    {
        yield return new WaitForSeconds(4f); // Biarkan overlay tampil selama 2 detik
        ShowFinanceSummary(); // Ganti dengan ShowFinanceSummary() jika tidak perlu EndingGameOver
    }


    IEnumerator TungguDanLanjut()
    {
        yield return new WaitForSeconds(4f); // Tunggu selama 4 detik setelah overlay muncul

        if (overlayPeringatan != null)
        {
            overlayPeringatan.SetActive(false); // Hilangkan overlay setelah 4 detik
        }

        // Pindah ke level berikutnya jika sudah selesai
        SceneManager.LoadScene("3DLV2"); // Ganti dengan nama scene level 2
    }

    IEnumerator ShowFinanceSummaryAfterDelay()
    {
        yield return new WaitForSeconds(delayToSummary);  // Menunggu selama delayToSummary
        ShowFinanceSummary();  // Tampilkan ringkasan keuangan setelah delay
    }

}
