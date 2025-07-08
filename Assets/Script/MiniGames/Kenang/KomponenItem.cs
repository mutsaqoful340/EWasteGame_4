using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KomponenItem : MonoBehaviour
{
    [Header("🧩 Data Komponen")]
    public string nama;
    [TextArea]
    public string deskripsi;
    public bool isKomponenBenar;

    // Tidak perlu drag Image/Text dari inspector manual
    private Image gambarUI;
    private TMP_Text namaText;

    public void Init(string _nama, string _deskripsi, Sprite _sprite, bool _isBenar)
    {
        nama = _nama;
        deskripsi = _deskripsi;
        isKomponenBenar = _isBenar;

        // Cari komponen UI otomatis dari anak-anak prefab
        if (gambarUI == null)
            gambarUI = GetComponentInChildren<Image>();

        if (namaText == null)
            namaText = GetComponentInChildren<TMP_Text>();

        if (gambarUI != null)
            gambarUI.sprite = _sprite;

        if (namaText != null)
            namaText.text = _nama;
    }
}
