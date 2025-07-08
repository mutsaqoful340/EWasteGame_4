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

    private Image gambarUI;
    private TMP_Text namaText;

    [HideInInspector] public Transform parentAwal;
    [HideInInspector] public Vector3 posisiAwal;

    private void Start()
    {
        // Simpan posisi awal saat pertama kali muncul
        parentAwal = transform.parent;
        posisiAwal = transform.localPosition;

        // Cari komponen UI otomatis dari anak
        if (gambarUI == null)
            gambarUI = GetComponentInChildren<Image>();

        if (namaText == null)
            namaText = GetComponentInChildren<TMP_Text>();
    }

    public void Init(string _nama, string _deskripsi, Sprite _sprite, bool _isBenar)
    {
        nama = _nama;
        deskripsi = _deskripsi;
        isKomponenBenar = _isBenar;

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
