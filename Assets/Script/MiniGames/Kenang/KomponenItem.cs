using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KomponenItem : MonoBehaviour
{
    [Header("🧩 Data Komponen")]
    public string nama;

    [TextArea]
    public string deskripsi;

    public bool isKomponenBenar; // True jika ini komponen laptop asli

    [Header("🖼️ Referensi UI")]
    public Image gambar;              // Gambar komponen (Image UI)
    public TMP_Text namaText;         // Nama komponen (TextMeshPro)

    /// <summary>
    /// Dipanggil oleh Spawner untuk inisialisasi data dan tampilan
    /// </summary>
    public void Init(string _nama, string _deskripsi, Sprite _sprite, bool _isBenar)
    {
        nama = _nama;
        deskripsi = _deskripsi;
        isKomponenBenar = _isBenar;

        if (gambar != null)
            gambar.sprite = _sprite;

        if (namaText != null)
            namaText.text = _nama;
    }
}
