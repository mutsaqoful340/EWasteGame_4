using UnityEngine;

public class HPDragHandler : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private bool isDragging = false;
    public LayerMask hpLayerMask;
    public BoxCollider kardusCollider;

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

                // Panggil TakeHP di script HPTakerKeyboardBoxAnim
                HPTakerKeyboardBoxAnim hpTakeScript = GetComponent<HPTakerKeyboardBoxAnim>();
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

                if (kardusCollider != null && kardusCollider.bounds.Contains(targetPos))
                {
                    Debug.Log("Posisi HP menabrak kardus, gerakan dibatalkan");
                }
                else
                {
                    transform.position = targetPos;
                }
            }
        }
    }
}
