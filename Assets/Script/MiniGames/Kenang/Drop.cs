using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public bool isSlotBenar;

    [Header("UI Panel Info")]
    public GameObject panelInfo;
    public TextMeshProUGUI namaText;
    public TextMeshProUGUI deskripsiText;
    public Image gambarItem;
    public Button closeButton;

    private void Start()
    {
        if (panelInfo != null)
            panelInfo.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;

        if (droppedObj != null)
        {
            KomponenItem item = droppedObj.GetComponent<KomponenItem>();
            if (item != null)
            {
                if (panelInfo != null)
                    panelInfo.SetActive(true);

                if (item.isKomponenBenar == isSlotBenar)
                {
                    // ✅ Drop benar
                    namaText.text = item.nama;
                    deskripsiText.text = item.deskripsi;

                    if (gambarItem != null)
                    {
                        Image draggedImage = droppedObj.GetComponentInChildren<Image>();
                        if (draggedImage != null)
                            gambarItem.sprite = draggedImage.sprite;
                    }

                    // Tempel dan posisikan
                    droppedObj.transform.SetParent(transform);
                    RectTransform rt = droppedObj.GetComponent<RectTransform>();
                    if (rt != null)
                        rt.anchoredPosition = Vector2.zero;

                    // Nonaktifkan drag
                    DragHandler11 drag = droppedObj.GetComponent<DragHandler11>();
                    if (drag != null)
                        drag.enabled = false;
                }
                else
                {
                    // ❌ Drop salah
                    namaText.text = "SALAH!";
                    deskripsiText.text = "Itu bukan tempatnya.";
                    if (gambarItem != null)
                        gambarItem.sprite = null;

                    // Kembalikan item ke posisi awal
                    droppedObj.transform.SetParent(item.parentAwal);
                    droppedObj.transform.localPosition = item.posisiAwal;
                }

                // Hitung bahwa satu item sudah masuk (tetap dihitung walau salah)
                GameManagerKenang.instance.TambahItemMasuk();
            }
        }
    }

    private void ClosePanel()
    {
        if (panelInfo != null)
            panelInfo.SetActive(false);

        if (GameManagerKenang.instance.sudahSelesai)
        {
            GameManagerKenang.instance.TampilkanPanelAkhir();
        }
    }
}
