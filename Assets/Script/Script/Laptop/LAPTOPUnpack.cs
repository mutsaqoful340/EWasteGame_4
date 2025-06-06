using UnityEngine;

public class LAPTOPUnpack : MonoBehaviour
{
    public GameObject Top_EnclosurePrefab;
    public GameObject LCDPrefab;
    public GameObject MachinePrefab;
    public GameObject SIMCardPrefab;
    public GameObject batteryPrefab;
    public GameObject Bottom_EnclosurePrefab;

    public GameObject kertasPrefab;
    public GameObject pensilPrefab;

    public Transform centerPoint;    // Ini posisi hp utama, assign di inspector
    public float radius = 0.1f;      // Jarak melingkar dari centerPoint

    public void SpawnItemAroundCenter(int slotIndex, string itemType, int totalItems)
    {
        if (centerPoint == null)
        {
            Debug.LogWarning("Center point belum diassign!");
            return;
        }

        GameObject prefabToSpawn = GetPrefabByType(itemType);
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Tipe item tidak dikenal: " + itemType);
            return;
        }

        // Hitung sudut berdasarkan indeks dan total item agar membentuk lingkaran
        float angleStep = 360f / totalItems;
        float angle = angleStep * slotIndex;
        float angleRad = angle * Mathf.Deg2Rad;

        // Hitung posisi spawn di lingkaran sekitar centerPoint
        Vector3 offset = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;
        Vector3 spawnPos = centerPoint.position + offset;

        // Tambahkan offset kecil berdasarkan tipe item supaya tidak tumpang tindih
        Vector3 extraOffset = GetOffsetByType(itemType);
        spawnPos += extraOffset;

        // Tambahkan offset tinggi supaya objek tidak nembus meja
        Renderer rend = prefabToSpawn.GetComponentInChildren<Renderer>();
        float yOffset = rend ? rend.bounds.extents.y : 0.02f;
        spawnPos.y += yOffset;

        // Rotasi supaya prefab baring menghadap ke atas
        Quaternion spawnRot = Quaternion.Euler(-90f, 0f, 0f);

        GameObject spawnedItem = Instantiate(prefabToSpawn, spawnPos, spawnRot);
        spawnedItem.name = prefabToSpawn.name + "_Spawned";

        // Jika prefab ada komponen DraggableItem, set itemType-nya
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
        float radius = 0.02f;
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
