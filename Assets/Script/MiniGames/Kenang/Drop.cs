using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public bool isSlotBenar;

    [Header("UI Tujuan (Panel Info)")]
    public TextMeshProUGUI namaText;
    public TextMeshProUGUI deskripsiText;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj != null)
        {
            KomponenItem item = droppedObj.GetComponent<KomponenItem>();
            if (item != null)
            {
                if (item.isKomponenBenar == isSlotBenar)
                {
                    // ✅ Tampilkan info benar
                    namaText.text = item.nama;
                    deskripsiText.text = item.deskripsi;

                    // ✅ Tempelkan objek ke slot
                    droppedObj.transform.SetParent(transform);

                    // ✅ Posisikan ke tengah slot
                    var rt = droppedObj.GetComponent<RectTransform>();
                    if (rt != null)
                        rt.anchoredPosition = Vector2.zero;

                    // ✅ Matikan drag
                    var drag = droppedObj.GetComponent<DragHandler11>();
                    if (drag != null)
                        drag.enabled = false;
                }
                else
                {
                    // ❌ Salah tempat
                    namaText.text = "SALAH!";
                    deskripsiText.text = "Itu bukan tempatnya.";
                }
            }
        }
    }
}
