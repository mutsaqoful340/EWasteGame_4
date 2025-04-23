using UnityEngine;
using System.Collections;

public class DraggableItem : MonoBehaviour
{
    public string itemType; // Contoh: "SDCard" atau "RAM"
    private Vector3 startPos; // Posisi awal spawn (samping HP)
    private bool isDragging = false;
    private float zOffset;

    // Posisi spawn awal item (samping HP)
    public Vector3 spawnPosition;

    void Start()
    {
        startPos = transform.position; // Mengatur posisi spawn pertama kali
        spawnPosition = startPos; // Menyimpan posisi spawn pertama kali
    }

    void OnMouseDown()
    {
        zOffset = Camera.main.WorldToScreenPoint(transform.position).z;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = zOffset;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = worldPos;
        }
    }

    void OnMouseUp()
    {
        isDragging = false; // Menandakan bahwa dragging telah selesai
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Masuk ke zona: " + other.tag);

        if (other.CompareTag("TrashZone"))
        {
            // Logika ketika item berada di dalam tong sampah
            if (itemType == "SmallEwaste")
            {
                Debug.Log("DITOLAK: SmallEwaste tidak boleh masuk ke TrashZone");
                StartCoroutine(BalikKeAwal()); // Panggil fungsi untuk mental balik ke posisi awal
            }
            else
            {
                Debug.Log("Item dibuang ke tempat sampah.");
                Destroy(gameObject); // Hanya jika item selain SmallEwaste
            }
        }
        else if (other.CompareTag("StorageZone"))
        {
            if (itemType == "SmallEwaste")
            {
                Debug.Log("Item berhasil disimpan ke StorageZone");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("DITOLAK: Bukan item penyimpanan.");
                StartCoroutine(BalikKeAwal()); // Panggil mental balik ke posisi awal
            }
        }
    }

    // Fungsi untuk mengembalikan item ke posisi spawn (samping HP) dengan efek smooth (lerp)
    IEnumerator BalikKeAwal()
    {
        float t = 0;
        Vector3 start = transform.position;

        // Proses mental balik
        while (t < 1)
        {
            t += Time.deltaTime * 3f; // Kecepatan gerakan balik (ubah angka 3f untuk kecepatan)
            transform.position = Vector3.Lerp(start, spawnPosition, t); // Mengembalikan posisi ke spawnPosition
            yield return null; // Tunggu di setiap frame
        }
    }
}
