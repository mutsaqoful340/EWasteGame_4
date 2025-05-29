using UnityEngine;

public class HPUnpack : MonoBehaviour
{
    public GameObject Top_EnclosurePrefab;
    public GameObject LCDPrefab;
    public GameObject MachinePrefab;
    public GameObject SIMCardPrefab;
    public GameObject batteryPrefab;
    public GameObject Bottom_EnclosurePrefab;

    public GameObject kertasPrefab;
    public GameObject pensilPrefab;

    public Transform[] spawnSlots; // Misalnya 1 slot = posisi tengah HP

    public void SpawnItem(int slotIndex, string itemType)
    {
        if (slotIndex < 0 || slotIndex >= spawnSlots.Length)
        {
            Debug.LogWarning("Index slot spawn tidak valid.");
            return;
        }

        Transform baseTransform = spawnSlots[slotIndex];
        Vector3 basePos = baseTransform.position;
        Quaternion baseRot = baseTransform.rotation; // Rotasi HP

        GameObject prefabToSpawn = GetPrefabByType(itemType);
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Tipe item tidak dikenal: " + itemType);
            return;
        }

        // Offset relatif terhadap orientasi HP
        Vector3 offset = GetOffsetByType(itemType);
        Vector3 spawnPos = basePos + baseTransform.TransformDirection(offset);

        GameObject spawnedItem = Instantiate(prefabToSpawn, spawnPos, baseRot); // Mengikuti orientasi HP

        var dragComp = spawnedItem.GetComponent<DraggableItem>();
        if (dragComp != null)
        {
            dragComp.itemType = itemType;
        }
    }

    private GameObject GetPrefabByType(string itemType)
    {
        switch (itemType)
        {
            case "Top_Enclosure": return Top_EnclosurePrefab;
            case "LCD": return LCDPrefab;
            case "Machine": return MachinePrefab;
            case "SIMCard": return SIMCardPrefab;
            case "battery": return batteryPrefab;
            case "Bottom_Enclosure": return Bottom_EnclosurePrefab;
            case "kertas": return kertasPrefab;
            case "pensil": return pensilPrefab;
            default: return null;
        }
    }

    private Vector3 GetOffsetByType(string itemType)
    {
        float radius = 0.25f;

        switch (itemType)
        {
            case "Top_Enclosure": return new Vector3(0, 0, radius);
            case "LCD": return new Vector3(radius, 0, 0);
            case "Machine": return new Vector3(-radius, 0, 0);
            case "SIMCard": return new Vector3(0, 0, -radius);
            case "battery": return new Vector3(0, 0, -radius * 1.5f);
            case "Bottom_Enclosure": return new Vector3(0, 0, radius * 1.5f);
            case "kertas": return new Vector3(radius * 0.7f, 0, radius * 0.7f);
            case "pensil": return new Vector3(-radius * 0.7f, 0, radius * 0.7f);
            default: return Vector3.zero;
        }
    }
}
