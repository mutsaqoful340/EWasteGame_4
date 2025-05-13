using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;     // Prefab yang akan di-spawn
    public Transform centerPoint;           // Titik pusat spawn
    public float radius = 1.5f;             // Jarak dari pusat ke posisi spawn
    private bool hasSpawned = false;        // Cegah spawn ulang

    void OnMouseDown()
    {
        if (!hasSpawned)
        {
            if (centerPoint == null) centerPoint = this.transform; // fallback ke object ini
            SpawnInCircle();
            hasSpawned = true;
        }
    }

    void SpawnInCircle()
    {
        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            Debug.LogError("Prefab belum diisi!");
            return;
        }

        int total = prefabsToSpawn.Length;
        float angleStep = 360f / total;

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];

            if (prefab == null)
            {
                Debug.LogWarning($"Prefab pada index {i} kosong!");
                continue;
            }

            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 spawnPos = centerPoint.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;


            GameObject spawned = Instantiate(prefab, spawnPos, Quaternion.Euler(90f, 0f, 0f));
            spawned.name = prefab.name + "_Spawned";
        }
    }
}
