using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class KomponenInfoPopup : MonoBehaviour
{
    [System.Serializable]
    public class InfoKomponen
    {
        public string namaKomponen;
        [TextArea] public string fungsi;
        [TextArea] public string bahaya;
        [TextArea] public string daurUlang;
    }

    public GameObject popupPanel;
    public TextMeshProUGUI judulText;
    public TextMeshProUGUI fungsiText;
    public TextMeshProUGUI bahayaText;
    public TextMeshProUGUI daurUlangText;
    public Button tombolOke;

    public List<InfoKomponen> daftarInfo;

    void Start()
    {
        popupPanel.SetActive(false);
        tombolOke.onClick.AddListener(() => popupPanel.SetActive(false));
    }

    public void TampilkanInfo(string namaKomponen)
    {
        InfoKomponen info = daftarInfo.Find(x => x.namaKomponen == namaKomponen);
        if (info != null)
        {
            popupPanel.SetActive(true);
            judulText.text = "📦 Komponen: " + info.namaKomponen;
            fungsiText.text = "🔧 Fungsi: " + info.fungsi;
            bahayaText.text = "⚠️ Bahaya: " + info.bahaya;
            daurUlangText.text = "♻️ Daur Ulang: " + info.daurUlang;
        }
    }
}
