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


    void Start()
    {

        timer = totalTime;

        if (PlayerPrefs.GetInt("BuffJajanAktif", 0) == 1)
        {
            timer += 300f; // Tambah waktu 5 menit
            PlayerPrefs.SetInt("BuffJajanAktif", 0); // Reset agar hanya 1 kali
        }

        if (SceneManager.GetActiveScene().name == "3DLV1")
        {
            PlayerPrefs.DeleteKey("SisaUang");
        }

        // Ambil sisa uang dari level sebelumnya
        int sisaUangDariLevelSebelumnya = PlayerPrefs.GetInt("SisaUang", 0);

        // Tambahkan reward awal level sekarang
        currentReward = sisaUangDariLevelSebelumnya + initialReward;

        UpdateTimerUI();
        UpdateMoneyUI();

        if (financeSummaryPanel != null)
            financeSummaryPanel.SetActive(false);
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

        if (timer <= 0f)
        {
            currentReward = 0;
        }

        ShowFinanceSummary();
    }

    void ShowFinanceSummary()
    {
        if (financeSummaryPanel != null)
            financeSummaryPanel.SetActive(true);

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

        // === JAJAN LOGIC ===
        if (toggleJajan.isOn && !buffJajanSudahDipakai && currentReward >= jajanCost)
        {
            currentReward -= jajanCost;
            timer += 300f;
            buffJajanSudahDipakai = true;
            Debug.Log("Buff jajan aktif! Waktu bertambah 5 menit.");

            PlayerPrefs.SetInt("BuffJajanAktif", 1); // hanya simpan jika benar-benar jajan
        }

        // === MAKAN LOGIC ===
        if (toggleMakan.isOn && currentReward >= makanCost)
        {
            currentReward -= makanCost;
            sudahMakanHariIni = true;
        }
        else
        {
            sudahMakanHariIni = false;
        }

        // === CEK MAKAN KEMARIN ===
        int kemarinTidakMakan = PlayerPrefs.GetInt("TidakMakanKemarin", 0);

        if (!sudahMakanHariIni)
        {
            PlayerPrefs.SetInt("TidakMakanKemarin", 1);

            if (overlayPeringatan != null)
                overlayPeringatan.SetActive(true);

            if (kemarinTidakMakan == 1)
            {
                if (overlayPeringatan != null)
                {
                    overlayPeringatan.SetActive(true);

                    // Aktifkan tombol dan beri listener
                    if (btnLihatRingkasan != null && btnAkhiriGame != null)
                    {
                        btnLihatRingkasan.onClick.RemoveAllListeners();
                        btnAkhiriGame.onClick.RemoveAllListeners();

                        btnLihatRingkasan.onClick.AddListener(() =>
                        {
                            overlayPeringatan.SetActive(false);
                            ShowFinanceSummary(); // Lanjut ke ringkasan keuangan
                        });

                        btnAkhiriGame.onClick.AddListener(() =>
                        {
                            overlayPeringatan.SetActive(false);
                            EndingGameOver(); // Langsung game over
                        });
                    }
                }

                return; // Hentikan eksekusi di sini, tunggu input pemain
            }
            else
            {
                timer = Mathf.Max(0, timer - 120);
                if (efekBuram != null)
                    efekBuram.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetInt("TidakMakanKemarin", 0);

            if (overlayPeringatan != null)
                overlayPeringatan.SetActive(false);
            if (efekBuram != null)
                efekBuram.SetActive(false);
        }

        PlayerPrefs.Save();

        jajanSudahDiterapkan = true;
        UpdateMoneyUI();
        UpdateTimerUI();
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

    void EndingGameOver()
    {
        isGameOver = true;

        if (endingPanel != null)
            endingPanel.SetActive(true);

        Time.timeScale = 0f; // Pause game
    }

    IEnumerator DelayEnding()
    {
        yield return new WaitForSeconds(2f); // Biarkan overlay tampil selama 2 detik
        EndingGameOver();
    }

}
