using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerKenang : MonoBehaviour
{
    public static GameManagerKenang instance;

    [Header("Panel UI")]
    public GameObject panelStart;
    public GameObject panelAkhir;

    [Header("Scene Berikutnya")]
    public string namaSceneBerikutnya;

    [Header("Jumlah Komponen")]
    public int totalItem = 10;
    private int currentDrop = 0;

    public bool sudahSelesai = false;
    private bool gameDimulai = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (panelStart != null)
            panelStart.SetActive(true);

        if (panelAkhir != null)
            panelAkhir.SetActive(false);
    }

    // Dipanggil oleh tombol "Start"
    public void MulaiPermainan()
    {
        gameDimulai = true;

        if (panelStart != null)
            panelStart.SetActive(false);
    }

    public void TambahItemMasuk()
    {
        if (!gameDimulai) return;

        currentDrop++;
        Debug.Log("Item masuk: " + currentDrop + "/" + totalItem);

        if (currentDrop >= totalItem)
        {
            Debug.Log("Semua item sudah masuk. Tunggu panel info ditutup.");
            sudahSelesai = true;
        }
    }

    public void TampilkanPanelAkhir()
    {
        if (panelAkhir != null)
            panelAkhir.SetActive(true);
    }

    public void NextScene()
    {
        if (!string.IsNullOrEmpty(namaSceneBerikutnya))
        {
            SceneManager.LoadScene(namaSceneBerikutnya);
        }
        else
        {
            Debug.LogWarning("Nama scene belum diisi di Inspector!");
        }
    }
}
