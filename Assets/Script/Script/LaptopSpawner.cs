using UnityEngine;

public class LaptopSpawner : MonoBehaviour
{
    public GameObject[] laptopComponents;  // Semua komponen laptop
    public Transform centerPoint;          // Titik pusat spawn
    public float radius = 1.5f;            // Radius setengah lingkaran
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
            gameObject.SetActive(false); // Laptop menghilang
        }
    }

    void SpawnComponents(int startIndex, int endIndex)
    {
        int spawnTotal = endIndex - startIndex;

        for (int i = startIndex; i < endIndex && i < laptopComponents.Length; i++)
        {
            if (laptopComponents[i] == null) continue;

            int relativeIndex = i - startIndex;
            Vector3 spawnPos = GetHalfCirclePosition(relativeIndex, spawnTotal);
            Instantiate(laptopComponents[i], spawnPos, Quaternion.Euler(90f, 0f, 0f));  // Menghadap atas
        }
    }

    Vector3 GetHalfCirclePosition(int index, int total)
    {
        // Menghitung sudut untuk setengah lingkaran dari -90 ke +90 derajat
        float angleStep = 180f / (total + 1);
        float angleDeg = -90f + angleStep * (index + 1);
        float angleRad = angleDeg * Mathf.Deg2Rad;

        // Pusat menghadap ke depan (Z+)
        return centerPoint.position + new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;
    }
}
