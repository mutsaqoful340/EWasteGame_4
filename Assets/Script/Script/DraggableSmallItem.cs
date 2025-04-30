using UnityEngine;
using System.Collections;

public class DraggableSmallItem : MonoBehaviour
{
    private Vector3 startPos; // Posisi awal objek
    private bool isDragging = false; // Status apakah objek sedang di-drag
    private float zOffset; // Offset untuk drag

    public string itemType; // Jenis item, misalnya "SDCard" atau "RAM"

    void Start()
    {
        startPos = transform.position; // Menyimpan posisi awal objek
    }

    // Ketika mouse ditekan pada objek
    void OnMouseDown()
    {
        zOffset = Camera.main.WorldToScreenPoint(transform.position).z;
        isDragging = true; // Menandakan objek sedang di-drag
    }

    // Ketika objek sedang di-drag
    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = zOffset;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = worldPos; // Memindahkan objek sesuai posisi mouse
        }
    }

    // Ketika mouse dilepas
    void OnMouseUp()
    {
        isDragging = false; // Menandakan objek berhenti di-drag
    }

    // Ketika objek bertabrakan dengan zona
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrashZone"))
        {
            // Kembalikan objek ke posisi awal jika masuk ke TrashZone
            Debug.Log(itemType + " masuk ke TrashZone dan kembali ke posisi awal.");
            StartCoroutine(BalikKeAwal()); // Kembalikan objek ke posisi awal
        }
        else if (other.CompareTag("StorageZone"))
        {
            // Hancurkan item yang masuk ke StorageZone
            Debug.Log(itemType + " masuk ke StorageZone dan dihancurkan.");
            Destroy(gameObject); // Menghancurkan objek yang masuk ke StorageZone
        }
    }

    // Fungsi untuk mengembalikan item ke posisi awal dengan smooth (lerp)
    private IEnumerator BalikKeAwal()
    {
        float t = 0;
        Vector3 start = transform.position; // Posisi awal objek

        // Gerakkan objek kembali ke posisi spawn dengan efek smooth
        while (t < 1)
        {
            t += Time.deltaTime * 3f; // Kecepatan pergerakan balik
            transform.position = Vector3.Lerp(start, startPos, t); // Gerakkan objek kembali
            yield return null; // Tunggu di setiap frame
        }
    }
}
