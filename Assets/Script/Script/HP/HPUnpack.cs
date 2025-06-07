using UnityEngine;

public class HPUnpack : MonoBehaviour
{
    public enum DeviceType { HP, Laptop }
    public DeviceType deviceType = DeviceType.HP;

    public GameObject Top_EnclosurePrefab;
    public GameObject LCDPrefab;
    public GameObject MachinePrefab;
    public GameObject SIMCardPrefab;
    public GameObject batteryPrefab;
    public GameObject Bottom_EnclosurePrefab;
    public GameObject kertasPrefab;
    public GameObject pensilPrefab;

    public Transform centerPoint;
    public float radius = 0.1f;

    // Grid config for laptop
    public int laptopColumns = 3;
    public float spacing = 0.05f;

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

        Vector3 spawnPos = Vector3.zero;

        if (deviceType == DeviceType.HP)
        {
            // POSISI MELINGKAR
            float angleStep = 360f / totalItems;
            float angle = angleStep * slotIndex;
            float angleRad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;
            spawnPos = centerPoint.position + offset + GetOffsetByType(itemType);
        }
        else if (deviceType == DeviceType.Laptop)
        {
            // POSISI GRID
            int row = slotIndex / laptopColumns;
            int col = slotIndex % laptopColumns;
            Vector3 offset = new Vector3(col * spacing, 0, -row * spacing);
            spawnPos = centerPoint.position + offset;
        }

        // Tinggi agar tidak nembus
        Renderer rend = prefabToSpawn.GetComponentInChildren<Renderer>();
        float yOffset = rend ? rend.bounds.extents.y : 0.02f;
        spawnPos.y += yOffset;

        // Rotasi: HP horizontal, Laptop bisa tetap
        Quaternion spawnRot = (deviceType == DeviceType.HP)
            ? Quaternion.Euler(-90f, 0f, 0f)
            : Quaternion.identity;

        GameObject spawnedItem = Instantiate(prefabToSpawn, spawnPos, spawnRot);
        spawnedItem.name = prefabToSpawn.name + "_Spawned";

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
