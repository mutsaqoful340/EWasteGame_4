using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class SampahTargetData
{
    public string tipeSampah;
    public List<Transform> targetPositions;
}

public class WasteZone : MonoBehaviour
{
    [Header("Tipe Sampah & Target Posisi")]
    public List<SampahTargetData> sampahTargetList = new List<SampahTargetData>();

    [Header("Referensi VN")]
    public VNDialogManager vnDialogManager;
    public List<VNDialog> dialogSetelahBuang;

    [Header("Panel Ringkasan")]
    public GameObject financeSummaryPanel;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI totalUangText;

    [Header("Reward Settings")]
    public int rewardPerTipe = 10000;

    [Header("Referensi BoxPenyimpanan (untuk pelanggaran)")]
    public BoxPenyimpanan boxPenyimpanan;

    [Header("Jumlah Kardus Diharapkan")]
    public int jumlahKardusYangDiharapkan = 3;

    private Dictionary<string, int> currentTargetIndexPerType = new Dictionary<string, int>();
    private int jumlahKardusYangSudahMasuk = 0;

    private bool alreadyTriggered = false;
    private bool isGameOver = false;

    private int currentReward = 0;
    private int uangSebelumnya = 0;

    private bool isMovingToTarget = false;
    private GameObject kardusYangMasuk;
    private Transform currentTarget;

    void Start()
    {
        uangSebelumnya = PlayerPrefs.GetInt("SisaUang", 0);
        currentReward = uangSebelumnya;
        Debug.Log($"💰 Uang awal dari level sebelumnya: Rp{uangSebelumnya:N0}");

        if (financeSummaryPanel != null)
            financeSummaryPanel.SetActive(false);

        foreach (var data in sampahTargetList)
        {
            Debug.Log($"📦 Tipe: {data.tipeSampah}, Target Count: {data.targetPositions.Count}");
            foreach (var pos in data.targetPositions)
            {
                if (pos != null)
                    Debug.Log($"   ↪ Posisi: {pos.name} di {pos.position}");
                else
                    Debug.LogError($"❌ Posisi NULL untuk tipe {data.tipeSampah}");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"🟢 OnTriggerEnter dengan: {other.name}");

        if (alreadyTriggered || isGameOver) return;

        var box = other.GetComponent<BoxLidAnimatorController>();
        if (box != null)
        {
            Debug.Log($"🔍 Terdeteksi box dengan tipe: {box.boxType}");

            var data = sampahTargetList.Find(d => d.tipeSampah == box.boxType);
            if (data != null)
            {
                alreadyTriggered = true;
                Debug.Log($"✅ Sampah cocok: {box.boxType}");

                StartCoroutine(ProsesKardusMasuk(other.gameObject, box.boxType, data));
            }
            else
            {
                Debug.Log($"❌ Sampah salah. Diberikan: {box.boxType}, tidak ada dalam daftar yang diterima.");
                if (boxPenyimpanan != null)
                {
                    string pesan = $"Sampah salah: {box.boxType} tidak dikenali dalam zona ini.";
                    boxPenyimpanan.CatatPelanggaran(pesan);
                }
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Objek masuk TIDAK punya BoxLidAnimatorController!");
        }
    }

    IEnumerator ProsesKardusMasuk(GameObject boxObj, string tipeSampah, SampahTargetData data)
    {
        yield return new WaitForSeconds(1.5f);
        kardusYangMasuk = boxObj;

        if (!currentTargetIndexPerType.ContainsKey(tipeSampah))
            currentTargetIndexPerType[tipeSampah] = 0;

        int idx = currentTargetIndexPerType[tipeSampah];

        if (idx < data.targetPositions.Count)
        {
            currentTarget = data.targetPositions[idx];
            currentTargetIndexPerType[tipeSampah]++;
        }
        else
        {
            Debug.LogWarning($"⚠️ Tidak cukup targetPositions untuk tipe {tipeSampah}, gunakan terakhir.");
            currentTarget = data.targetPositions[data.targetPositions.Count - 1];
        }

        Debug.Log($"📦 Kardus ({tipeSampah}) diarahkan ke target {currentTarget.name}");
        isMovingToTarget = true;

        var rb = kardusYangMasuk.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            Debug.Log("🔧 Rigidbody diset kinematic supaya bisa dipindah manual.");
        }
    }

    void Update()
    {
        if (isMovingToTarget && kardusYangMasuk != null && currentTarget != null)
        {
            float speed = 3f;
            kardusYangMasuk.transform.position = Vector3.MoveTowards(
                kardusYangMasuk.transform.position,
                currentTarget.position,
                speed * Time.deltaTime
            );

            float distance = Vector3.Distance(kardusYangMasuk.transform.position, currentTarget.position);
            if (distance < 0.05f)
            {
                Debug.Log("✅ Kardus sampai ke target.");
                isMovingToTarget = false;
                LanjutkanSetelahPindah();
            }
        }
    }

    void LanjutkanSetelahPindah()
    {
        currentReward += rewardPerTipe;
        PlayerPrefs.SetInt("SisaUang", currentReward);
        PlayerPrefs.Save();

        jumlahKardusYangSudahMasuk++;
        Debug.Log($"✅ Kardus ke-{jumlahKardusYangSudahMasuk} berhasil masuk.");

        alreadyTriggered = false;

        if (jumlahKardusYangSudahMasuk >= jumlahKardusYangDiharapkan)
        {
            isGameOver = true;

            Debug.Log("🎉 Semua kardus sudah masuk. Lanjut ke VN atau ringkasan.");

            if (vnDialogManager != null && dialogSetelahBuang != null && dialogSetelahBuang.Count > 0)
            {
                vnDialogManager.isVNEnding = true;
                vnDialogManager.StartVN(dialogSetelahBuang);
            }
            else
            {
                ShowFinanceSummary();
            }
        }
    }

    private void ShowFinanceSummary()
    {
        currentReward = PlayerPrefs.GetInt("SisaUang", 0);
        Debug.Log($"📥 ShowFinanceSummary - currentReward: Rp{currentReward:N0}");

        if (financeSummaryPanel != null)
            financeSummaryPanel.SetActive(true);

        int tabunganSebelumnya = PlayerPrefs.GetInt("Tabungan", 0);

        if (rewardText != null)
            rewardText.text = "Tabungan: Rp" + tabunganSebelumnya.ToString("N0");

        if (totalUangText != null)
            totalUangText.text = "Total Uang: Rp" + currentReward.ToString("N0");

        if (boxPenyimpanan != null)
        {
            var pelanggaranList = boxPenyimpanan.GetPelanggaranList();
            if (pelanggaranList.Count > 0)
            {
                Debug.Log("📛 Ringkasan Pelanggaran:");
                foreach (var p in pelanggaranList)
                {
                    Debug.Log($"- {p}");
                }
            }
        }

        Toggle toggleMakan = GameObject.Find("ToggleMakan")?.GetComponent<Toggle>();
        Toggle toggleNabung = GameObject.Find("ToggleNabung")?.GetComponent<Toggle>();
        Toggle toggleJajan = GameObject.Find("ToggleJajan")?.GetComponent<Toggle>();
        TextMeshProUGUI sisaText = GameObject.Find("SisaText")?.GetComponent<TextMeshProUGUI>();

        if (toggleMakan != null && toggleNabung != null && toggleJajan != null && sisaText != null)
        {
            System.Action updateSisa = () =>
            {
                int sisa = currentReward;
                if (toggleMakan.isOn) sisa -= 10000;
                if (toggleNabung.isOn) sisa -= 15000;
                if (toggleJajan.isOn) sisa -= 5000;
                sisa = Mathf.Max(0, sisa);
                sisaText.text = "Sisa: Rp" + sisa.ToString("N0");
                Debug.Log($"🔁 Update Sisa: Rp{sisa:N0}");
            };

            toggleMakan.onValueChanged.RemoveAllListeners();
            toggleMakan.onValueChanged.AddListener((_) => updateSisa());

            toggleNabung.onValueChanged.RemoveAllListeners();
            toggleNabung.onValueChanged.AddListener((_) => updateSisa());

            toggleJajan.onValueChanged.RemoveAllListeners();
            toggleJajan.onValueChanged.AddListener((_) => updateSisa());

            updateSisa();
        }
    }

    public void TampilkanRingkasanLangsungDariVN()
    {
        Debug.Log("📋 Ringkasan dipanggil setelah VN selesai.");
        ShowFinanceSummary();
    }
}
