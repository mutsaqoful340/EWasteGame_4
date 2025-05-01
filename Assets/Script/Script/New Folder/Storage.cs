using UnityEngine;

public class BarangBin : MonoBehaviour
{
    public string acceptedTag = "Barang"; // Hanya menerima objek dengan tag "Barang"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(acceptedTag))
        {
            Debug.Log("Barang berhasil dibuang ke tempat sampah!");
            Destroy(other.gameObject); // Hapus barang setelah dibuang
        }
        else
        {
            Debug.Log("Item ini tidak bisa dibuang di tempat sampah ini.");
        }
    }
}
