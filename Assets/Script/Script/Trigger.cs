using UnityEngine;

public class Trigger : MonoBehaviour
{
    public BoxPenyimpanan boxPenyimpanan; // Pastikan ini di-assign di Inspector

    // Fungsi ini bisa dipanggil ketika objek menyentuh trigger atau saat ada interaksi
    private void OnTriggerEnter(Collider other)
    {
        // Memastikan item yang tepat dimasukkan
        if (other.CompareTag("Top_Enclosure"))  // Periksa tag atau tipe objek
        {
            boxPenyimpanan.AddItem("Top_Enclosure");  // Menambahkan item "Top_Enclosure"
        }
        else if (other.CompareTag("LCD"))
        {
            boxPenyimpanan.AddItem("LCD");  // Menambahkan item "LCD"
        }
        // Tambahkan else-if lainnya sesuai dengan item lain yang bisa dimasukkan
    }
}
