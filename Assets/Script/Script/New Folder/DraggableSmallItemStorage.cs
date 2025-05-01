using UnityEngine;
using System.Collections;

public class DraggableStorageItem : MonoBehaviour
{
    private Vector3 startPos;
    private bool isDragging = false;
    private float zOffset;

    public string itemType; // misal: "SmallEWaste"

    private BoxPenyimpanan boxManager;

    void Start()
    {
        startPos = transform.position;

        // Cari BoxPenyimpanan di scene
        boxManager = FindObjectOfType<BoxPenyimpanan>();
        if (boxManager == null)
        {
            Debug.LogError("BoxPenyimpanan tidak ditemukan di scene.");
        }
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
                // Pastikan jumlah belum penuh sebelum dihancurkan
                if (!boxManager.IsFull())
                {
                    boxManager.AddItem(itemType);
                    Debug.Log(itemType + " diterima di StorageZone dan dihancurkan.");
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("StorageZone sudah penuh, " + itemType + " tidak diterima.");
                    StartCoroutine(BalikKeAwal());
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
        Vector3 start = transform.position;

        while (t < 1)
        {
            t += Time.deltaTime * 3f;
            transform.position = Vector3.Lerp(start, startPos, t);
            yield return null;
        }
    }
}
