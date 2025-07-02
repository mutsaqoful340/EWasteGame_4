using UnityEngine;

public class HPDragHandler : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private bool isDragging = false;
    public LayerMask hpLayerMask;

    void Start()
    {
        cam = Camera.main;
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

                var hpTakeScript = GetComponent<HPTakerKeyboardBoxAnim>();
                if (hpTakeScript != null && !hpTakeScript.isTaken)
                {
                    hpTakeScript.TakeHP();
                }

                transform.SetParent(null);
                Debug.Log("Selesai drag HP, TakeHP dijalankan");
            }
        }

        if (isDragging)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Vector3 targetPos = hit.point + offset;

                // Ambil collider HP
                Collider hpCollider = GetComponent<Collider>();
                if (hpCollider == null) return;

                Vector3 halfExtents = hpCollider.bounds.extents;

                // Batasi pergerakan ke bawah agar tidak tembus alas
                LayerMask alasMask = LayerMask.GetMask("Alas");

                Collider[] hitAlas = Physics.OverlapBox(
                    targetPos,
                    halfExtents,
                    Quaternion.identity,
                    alasMask
                );

                if (hitAlas.Length > 0)
                {
                    // Cegah turun, tetap gunakan posisi X dan Z, tapi Y dipertahankan
                    targetPos.y = transform.position.y;
                    Debug.Log("Terdeteksi tabrakan dengan Alas, Y tetap");
                }

                transform.position = targetPos;
            }
        }
    }
}
