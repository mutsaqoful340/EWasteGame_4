using UnityEngine;

public class LaptopSpawner : MonoBehaviour
{
    public GameObject[] laptopComponents;
    public Transform centerPoint;
    public float radius = 1.5f;
    public float elevation = 0.1f; // Tambahan tinggi agar tidak menempel tanah
    private int clickCount = 0;

    void OnMouseDown()
    {
        clickCount++;

        if (centerPoint == null)
            centerPoint = this.transform;

        if (clickCount == 1)
        {
            SpawnComponents(0, 1);
        }
        else if (clickCount == 2)
        {
            SpawnComponents(1, 3);
        }
        else if (clickCount == 3)
        {
            SpawnComponents(3, laptopComponents.Length);
            gameObject.SetActive(false);
        }
    }

    void SpawnComponents(int startIndex, int endIndex)
    {
        int spawnTotal = endIndex - startIndex;

        for (int i = startIndex; i < endIndex && i < laptopComponents.Length; i++)
        {
            if (laptopComponents[i] == null) continue;

            int relativeIndex = i - startIndex;
            Vector3 spawnPos = GetCirclePosition(relativeIndex, spawnTotal);

            // Tambahkan sedikit tinggi agar tidak menempel ke lantai
            spawnPos.y += elevation;

            // Hadap ke pusat
            Vector3 direction = (centerPoint.position - spawnPos).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);

            Instantiate(laptopComponents[i], spawnPos, lookRotation);
        }
    }

    Vector3 GetCirclePosition(int index, int total)
    {
        float angleStep = 360f / total;
        float angleDeg = angleStep * index;
        float angleRad = angleDeg * Mathf.Deg2Rad;

        // Posisi sekeliling pusat, tetap di satu bidang (Y sama)
        return centerPoint.position + new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;
    }
}
