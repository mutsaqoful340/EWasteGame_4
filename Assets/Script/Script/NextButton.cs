using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NextButton : MonoBehaviour
{
    public TextMeshProUGUI sisaUangText; // Referensi ke sisaUangText
    public BoxPenyimpanan boxPenyimpanan; // Tambahkan referensi ke script BoxPenyimpanan

    public void OnNextLevelButtonPressed()
    {
        // Terapkan pilihan dulu sebelum lanjut
        if (boxPenyimpanan != null)
        {
            boxPenyimpanan.TerapkanPilihan();
        }

        // Ambil sisa uang dari UI
        int sisa = int.Parse(sisaUangText.text.Split(' ')[1].Replace("Rp", "").Replace(",", ""));

        // Simpan sisa uang ke PlayerPrefs
        PlayerPrefs.SetInt("SisaUang", sisa);
        PlayerPrefs.Save();

        // Pindah ke level berikutnya (3DLV2)
        SceneManager.LoadScene("3DLV2"); // Ganti dengan nama scene level 2 kamu
    }
}