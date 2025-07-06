using UnityEngine;



public class TrashBin : MonoBehaviour
{
    public string acceptedTag = "Ewaste"; // Ganti sesuai tag objek yang ingin dibuang

    public BoxPenyimpanan boxPenyimpanan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(acceptedTag))
        {
            Debug.Log("Item berhasil dibuang ke tong sampah!");

            // Panggil AddItem di BoxPenyimpanan
            if (boxPenyimpanan != null)
            {
                boxPenyimpanan.AddItem(other.tag); // atau other.name jika kamu ingin lebih spesifik
            }

            Destroy(other.gameObject); // Hapus item setelah dibuang
        }
        else
        {
            Debug.Log("Item ini tidak bisa dibuang di tong ini.");
        }
    }

}
