using System.Collections;
using UnityEngine;

public class DraggableStorageItem : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 lastSafePos;
    private bool isDragging = false;
    private float zOffset;
    private float startY;

    public string itemType; // misal: "SmallEWaste"

    // Support kedua jenis Box
    private BoxPenyimpanan boxManager;
    private BoxPenyimpananSimple simpleBoxManager;

    void Start()
    {
        startPos = transform.position;
        lastSafePos = startPos;
        startY = startPos.y;

        boxManager = FindObjectOfType<BoxPenyimpanan>();
        simpleBoxManager = FindObjectOfType<BoxPenyimpananSimple>();

        if (boxManager == null && simpleBoxManager == null)
        {
            Debug.LogError("Tidak ditemukan BoxPenyimpanan atau BoxPenyimpananSimple di scene.");
        }
    }

    void OnMouseDown()
    {
        zOffset = Camera.main.WorldToScreenPoint(transform.position).z;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zOffset;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.y = startY;

        Collider myCollider = GetComponent<Collider>();
        if (myCollider == null) return;

        // Kurangi ukuran deteksi agar tidak terlalu sensitif
        Vector3 halfExtents = myCollider.bounds.extents * 0.7f;

        LayerMask obstacleLayers = LayerMask.GetMask("Kardus", "Meja", "Alas");

        Collider[] hits = Physics.OverlapBox(worldPos, halfExtents, Quaternion.identity, obstacleLayers);

        if (hits.Length > 0)
        {
            Debug.Log("❌ Tabrakan dengan: " + hits[0].name + ", posisi dibatalkan.");
            transform.position = lastSafePos;
            return;
        }

        // Aman → gerakkan objek dan simpan posisi valid
        transform.position = worldPos;
        lastSafePos = worldPos;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrashZone"))
        {
            Debug.Log(itemType + " ditolak di TrashZone dan kembali ke posisi awal.");
            StartCoroutine(BalikKeAwal());
        }
        else if (other.CompareTag("StorageZone"))
        {
            if (gameObject.CompareTag("SmallEWaste"))
            {
                if (boxManager != null)
                {
                    if (!boxManager.IsFull())
                    {
                        boxManager.AddItem(itemType);
                        Debug.Log(itemType + " diterima di StorageZone (BoxPenyimpanan) dan dihancurkan.");
                        Destroy(gameObject);
                    }
                    else
                    {
                        Debug.Log("BoxPenyimpanan penuh, tidak bisa menambahkan item.");
                        StartCoroutine(BalikKeAwal());
                    }
                }
                else if (simpleBoxManager != null)
                {
                    if (!simpleBoxManager.IsFull())
                    {
                        simpleBoxManager.AddItem();
                        Debug.Log(itemType + " diterima di StorageZone (Simple) dan dihancurkan.");
                        Destroy(gameObject);
                    }
                    else
                    {
                        Debug.Log("BoxPenyimpananSimple penuh, tidak bisa menambahkan item.");
                        StartCoroutine(BalikKeAwal());
                    }
                }
            }
            else
            {
                Debug.Log(itemType + " tidak cocok untuk StorageZone.");
                StartCoroutine(BalikKeAwal());
            }
        }
    }

    private IEnumerator BalikKeAwal()
    {
        float t = 0;
        Vector3 currentPos = transform.position;

        while (t < 1)
        {
            t += Time.deltaTime * 3f;
            transform.position = Vector3.Lerp(currentPos, startPos, t);
            yield return null;
        }
    }
}
