using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    public BoxPenyimpanan boxPenyimpanan;

    void OnTriggerEnter(Collider other)
    {
        // Cek apakah object yang masuk adalah item yang boleh masuk
        if (other.CompareTag("Item")) // ganti "Item" sesuai tag item kamu
        {
            if (!boxPenyimpanan.IsFull())
            {
                boxPenyimpanan.AddItem();
                Destroy(other.gameObject); // buang item agar tidak nabrak ulang
            }
            else
            {
                Debug.Log("Box penuh. Item tidak diterima.");
            }
        }
    }
}
