using UnityEngine;

public class HPUnpack : MonoBehaviour
{
    public GameObject Top_EnclosurePrefab;
    public GameObject LCDPrefab;
    public GameObject MachinePrefab;
    public GameObject SIMCardPrefab;
    public GameObject batteryPrefab;
    public GameObject Bottom_EnclosurePrefab;

    // Tambahan prefab baru
    public GameObject kertasPrefab;
    public GameObject pensilPrefab;

    public Transform[] spawnSlots; // Slot-slot spawn

    // Fungsi untuk spawn item berdasarkan slot dan tipe
    public void SpawnItem(int slotIndex, string itemType)
    {
        if (slotIndex < 0 || slotIndex >= spawnSlots.Length)
        {
            Debug.LogWarning("Index slot spawn tidak valid.");
            return;
        }

        Vector3 spawnPos = spawnSlots[slotIndex].position;
        GameObject spawnedItem = null;

        switch (itemType)
        {
            case "Top_Enclosure":
                spawnedItem = Instantiate(Top_EnclosurePrefab, spawnPos, Quaternion.identity);
                break;
            case "LCD":
                spawnedItem = Instantiate(LCDPrefab, spawnPos, Quaternion.identity);
                break;
            case "Machine":
                spawnedItem = Instantiate(MachinePrefab, spawnPos, Quaternion.identity);
                break;
            case "SIMCard":
                spawnedItem = Instantiate(SIMCardPrefab, spawnPos, Quaternion.identity);
                break;
            case "battery":
                spawnedItem = Instantiate(batteryPrefab, spawnPos, Quaternion.identity);
                break;
            case "Bottom_Enclosure":
                spawnedItem = Instantiate(Bottom_EnclosurePrefab, spawnPos, Quaternion.identity);
                break;
            case "kertas":
                spawnedItem = Instantiate(kertasPrefab, spawnPos, Quaternion.identity);
                break;
            case "pensil":
                spawnedItem = Instantiate(pensilPrefab, spawnPos, Quaternion.identity);
                break;
            default:
                Debug.LogWarning("Tipe item tidak dikenal: " + itemType);
                break;
        }

        if (spawnedItem != null)
        {
            var dragComp = spawnedItem.GetComponent<DraggableItem>();
            if (dragComp != null)
            {
                dragComp.itemType = itemType;
            }
        }
    }
}
