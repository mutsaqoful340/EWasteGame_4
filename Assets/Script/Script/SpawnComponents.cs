using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;     // Prefab yang akan di-spawn
    public Transform centerPoint;           // Titik pusat spawn (misalnya HP)
    public float radius = 1.5f;             // Jarak dari pusat ke posisi spawn
    private bool hasSpawned = false;        // Cegah spawn ulang

    void OnMouseDown()
    {
        if (!hasSpawned)
        {
            if (centerPoint == null) centerPoint = this.transform; // fallback
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

            // Hitung posisi offset melingkar lokal
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 localOffset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 spawnPos = centerPoint.position + centerPoint.TransformDirection(localOffset);

            // Gunakan rotasi HP
            Quaternion spawnRot = centerPoint.rotation;

            // Instansiasi prefab tapi disable dulu untuk ukur tinggi
            GameObject spawned = Instantiate(prefab, spawnPos, spawnRot);
            spawned.name = prefab.name + "_Spawned";
            spawned.SetActive(false); // disable sementara

            // Hitung offset Y berdasarkan ukuran objek
            float yOffset = GetHeightOffset(spawned);
            spawnPos += centerPoint.up * yOffset;

            // Pindahkan posisi baru dan aktifkan
            spawned.transform.position = spawnPos;
            spawned.SetActive(true);
        }
    }

    float GetHeightOffset(GameObject go)
    {
        Renderer rend = go.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            return rend.bounds.extents.y;
        }
        else
        {
            return 0.1f; // fallback default kalau tidak ada Renderer
        }
    }
}
