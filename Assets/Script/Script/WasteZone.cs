using System.Collections.Generic;
using UnityEngine;

public class WasteZone : MonoBehaviour
{
    [Header("Tipe sampah yang diterima")]
    public string acceptedType = "ewaste";

    [Header("Referensi VN")]
    public VNDialogManager vnDialogManager; // VN Manager yang aktif di scene
    public List<VNDialog> dialogSetelahBuang; // Dialog yang akan muncul setelah sampah cocok

    private bool alreadyTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyTriggered) return;

        var box = other.GetComponent<BoxLidAnimatorController>();
        if (box != null)
        {
            if (box.boxType == acceptedType)
            {
                alreadyTriggered = true;
                Debug.Log($"✅ Sampah cocok: {box.boxType}");

                // Mulai Visual Novel Dialog
                if (vnDialogManager != null)
                {
                    if (dialogSetelahBuang != null && dialogSetelahBuang.Count > 0)
                    {
                        vnDialogManager.StartVN(dialogSetelahBuang);
                    }
                    else
                    {
                        Debug.LogWarning("❗ List VN dialog kosong!");
                    }
                }
                else
                {
                    Debug.LogWarning("❗ VNDialogManager belum di-assign di Inspector!");
                }
            }
            else
            {
                Debug.Log($"❌ Sampah salah. Diberikan: {box.boxType}, diterima: {acceptedType}");
                // Tambah feedback gagal (optional)
            }
        }
    }
}
