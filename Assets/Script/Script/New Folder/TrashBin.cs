using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public string acceptedTag = "Ewaste"; // Ganti sesuai tag objek yang ingin dibuang

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(acceptedTag))
        {
            Debug.Log("Item berhasil dibuang ke tong sampah!");
            Destroy(other.gameObject); // Hapus item setelah dibuang
        }
        else
        {
            Debug.Log("Item ini tidak bisa dibuang di tong ini.");
        }
    }
}
