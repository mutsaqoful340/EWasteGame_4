using UnityEngine;

public class LaptopSpawner : MonoBehaviour
{
    public GameObject[] laptopComponents;  // Semua komponen laptop
    public Transform centerPoint;          // Titik pusat spawn
    public float radius = 1.5f;            // Jarak dari pusat
    private int clickCount = 0;            // Hitung klik

    void OnMouseDown()
    {
        clickCount++;

        if (centerPoint == null)
            centerPoint = this.transform;

        if (clickCount == 1)
        {
            SpawnComponents(0, 1);  // Spawn 1 komponen pertama
        }
        else if (clickCount == 2)
        {
            SpawnComponents(1, 3);  // Spawn 2 komponen berikutnya (index 1 dan 2)
        }
        else if (clickCount == 3)
        {
            SpawnComponents(3, laptopComponents.Length);  // Spawn sisanya
            gameObject.SetActive(false); // Laptop menghilang
        }
    }

    void SpawnComponents(int startIndex, int endIndex)
    {
        for (int i = startIndex; i < endIndex && i < laptopComponents.Length; i++)
        {
            if (laptopComponents[i] == null) continue;

            Vector3 spawnPos = GetPositionAroundCircle(i, laptopComponents.Length);
            Instantiate(laptopComponents[i], spawnPos, Quaternion.Euler(90f, 0f, 0f));  // menghadap atas
        }
    }

    Vector3 GetPositionAroundCircle(int index, int total)
    {
        float angleStep = 360f / total;
        float angle = angleStep * index * Mathf.Deg2Rad;
        return centerPoint.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
    }
}
