using UnityEngine;
using System.Collections.Generic;

public class HPUnpack : MonoBehaviour
{
    public enum DeviceType { HP, Laptop }
    public DeviceType deviceType = DeviceType.HP;

    public List<GameObject> itemPrefabs = new List<GameObject>();

    public Transform centerPoint;
    public float radius = 0.1f;

    // Grid config for laptop
    public int laptopColumns = 3;
    public float spacing = 0.05f;

    public void SpawnAllItems()
    {
        if (centerPoint == null)
        {
            Debug.LogWarning("Center point belum diassign!");
            return;
        }

        int totalItems = itemPrefabs.Count;

        for (int i = 0; i < totalItems; i++)
        {
            GameObject prefabToSpawn = itemPrefabs[i];
            if (prefabToSpawn == null) continue;

            Vector3 spawnPos = Vector3.zero;

            if (deviceType == DeviceType.HP)
            {
                float angleStep = 360f / totalItems;
                float angle = angleStep * i;
                float angleRad = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;
                spawnPos = centerPoint.position + offset;
            }
            else if (deviceType == DeviceType.Laptop)
            {
                int row = i / laptopColumns;
                int col = i % laptopColumns;
                Vector3 offset = new Vector3(col * spacing, 0, -row * spacing);
                spawnPos = centerPoint.position + offset;
            }

            Renderer rend = prefabToSpawn.GetComponentInChildren<Renderer>();
            float yOffset = rend ? rend.bounds.extents.y : 0.02f;
            spawnPos.y += yOffset;

            Quaternion spawnRot = (deviceType == DeviceType.HP)
                ? Quaternion.Euler(-90f, 0f, 0f)
                : Quaternion.Euler(90f, 0f, 0f);

            GameObject spawnedItem = Instantiate(prefabToSpawn, spawnPos, spawnRot);
            spawnedItem.name = prefabToSpawn.name + "_Spawned";
        }
    }
}
