using UnityEngine;

public class SpawnOnClickLAPTOP : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    public Transform centerPoint;
    public float radius = 1.0f;
    private bool hasSpawned = false;

    void OnMouseDown()
    {
        if (!hasSpawned)
        {
            if (centerPoint == null) centerPoint = this.transform;
            SpawnFlatCircle();
            hasSpawned = true;
        }
    }

    void SpawnFlatCircle()
    {
        int total = prefabsToSpawn.Length;
        if (total == 0) return;

        float angleStep = 360f / total;
        float yOffset = 0.0f; // Bisa sesuaikan nanti jika terlalu tinggi

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];
            if (prefab == null) continue;

            float angleRad = angleStep * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;

            Vector3 spawnPos = centerPoint.position + offset + Vector3.up * yOffset;

            // Rotasi agar objek rata di atas lantai
            Quaternion flatRotation = Quaternion.Euler(90f, 0f, 0f);

            Instantiate(prefab, spawnPos, flatRotation);
        }
    }


}
