using UnityEngine;

public class HPDragHandler : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private bool isDragging = false;
    public LayerMask hpLayerMask;
    public LayerMask alasLayerMask; // <-- Tambahkan LayerMask untuk Alas

    private float minimumY; // Y alas yang akan dijaga

    void Start()
    {
        cam = Camera.main;

        // Cari posisi Y alas secara otomatis (gunakan Raycast ke bawah dari HP awal)
        Ray downRay = new Ray(transform.position + Vector3.up * 2f, Vector3.down);
        if (Physics.Raycast(downRay, out RaycastHit hit, 10f, alasLayerMask))
        {
            minimumY = hit.point.y + GetComponent<Collider>().bounds.extents.y;
        }
        else
        {
            minimumY = transform.position.y; // fallback
            Debug.LogWarning("Tidak menemukan Alas, minimumY diset ke posisi awal");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, hpLayerMask))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    offset = transform.position - hit.point;
                    isDragging = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isDragging = false;

                // Jalankan aksi ambil HP
                HPTakerKeyboardBoxAnim hpTakeScript = GetComponent<HPTakerKeyboardBoxAnim>();
                if (hpTakeScript != null && !hpTakeScript.isTaken)
                {
                    hpTakeScript.TakeHP();
                }

                transform.SetParent(null);
            }
        }

        if (isDragging)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Vector3 targetPos = hit.point + offset;

                // Jaga agar Y tidak lebih rendah dari minimum
                if (targetPos.y < minimumY)
                {
                    targetPos.y = minimumY;
                }

                transform.position = targetPos;
            }
        }
    }
}
