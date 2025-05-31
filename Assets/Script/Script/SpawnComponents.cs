using UnityEngine;

public class SpawnOnClick : MonoBehaviour
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
        float yOffset = -0.3f; // coba sesuaikan nilai ini

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];
            if (prefab == null) continue;

            float angleRad = angleStep * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;

            Vector3 spawnPos = centerPoint.position + offset + Vector3.up * yOffset;

            Quaternion flatRotation = centerPoint.rotation * Quaternion.Euler(-90f, 0f, 0f);

            Instantiate(prefab, spawnPos, flatRotation);
        }
    }

}
