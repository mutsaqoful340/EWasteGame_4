using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{
    public enum SpawnType { HP, Laptop }
    public SpawnType spawnType = SpawnType.HP;

    public GameObject[] prefabsToSpawn;
    public Transform centerPoint;
    public float radius = 1.0f;
    private bool hasSpawned = false;

    // Grid settings for laptop
    public int laptopColumns = 3;
    public float spacing = 0.5f;

    void OnMouseDown()
    {
        if (!hasSpawned)
        {
            if (centerPoint == null) centerPoint = this.transform;

            if (spawnType == SpawnType.HP)
            {
                SpawnFlatCircle();
            }
            else if (spawnType == SpawnType.Laptop)
            {
                SpawnLaptopComponents();
            }

            hasSpawned = true;
        }
    }

    void SpawnFlatCircle()
    {
        int total = prefabsToSpawn.Length;
        if (total == 0) return;

        float angleStep = 360f / total;
        float yOffset = 0.0f;

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];
            if (prefab == null) continue;

            float angleRad = angleStep * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;
            Vector3 spawnPos = centerPoint.position + offset + Vector3.up * yOffset;

            Quaternion flatRotation = Quaternion.Euler(90f, 0f, 0f);
            Instantiate(prefab, spawnPos, flatRotation);
        }
    }

    void SpawnLaptopComponents()
    {
        int total = prefabsToSpawn.Length;
        if (total == 0) return;

        float yOffset = 0.0f;

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];
            if (prefab == null) continue;

            int row = i / laptopColumns;
            int col = i % laptopColumns;

            Vector3 offset = new Vector3(col * spacing, 0, -row * spacing);
            Vector3 spawnPos = centerPoint.position + offset + Vector3.up * yOffset;

            // Diperbarui: arahkan ke atas seperti pada HP
            Quaternion flatRotation = Quaternion.Euler(90f, 0f, 0f);
            Instantiate(prefab, spawnPos, flatRotation);
        }
    }
}
