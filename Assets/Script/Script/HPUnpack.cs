using UnityEngine;

public class HPUnpack : MonoBehaviour
{
    public GameObject Top_EnclosurePrefab;     // Prefab Top Enclosure
    public GameObject LCDPrefab;               // Prefab LCD
    public GameObject MachinePrefab;           // Prefab Machine
    public GameObject SIMCardPrefab;           // Prefab SIM Card
    public GameObject batteryPrefab;           // Prefab Battery
    public GameObject Bottom_EnclosurePrefab;  // Prefab Bottom Enclosure

    // Fungsi untuk spawn item berdasarkan tipe
    public void SpawnItem(Vector3 spawnPos, string itemType)
    {
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
