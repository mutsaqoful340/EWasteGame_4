using UnityEngine;

public class BoxLidAnimatorController : MonoBehaviour
{
    public Animator boxAnimator;
    public Transform targetPosition;
    public float moveSpeed = 2f;

    public string boxType = "ewasteBox"; // Bisa kamu ubah jadi enum kalau mau

    private bool isOpen = false;
    private bool isMoved = false;
    private bool isMoving = false;
    private bool canDrag = false;
    private bool isDragging = false;

    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // --- Handle Lid Movement ---
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
            {
                transform.position = targetPosition.position;
                isMoving = false;
                isMoved = true;
                Debug.Log("Kardus sudah sampai titik tujuan.");
            }
        }

        // --- Raycast Click Detection ---
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    if (canDrag)
                    {
                        offset = transform.position - GetMouseWorldPos();
                        isDragging = true;
                    }
                    else
                    {
                        ToggleBox();
                    }
                }
            }
        }

        // --- Handle Drag ---
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 newPos = GetMouseWorldPos() + offset;
            transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
        }

        // --- Stop Drag ---
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(mousePos);
    }

    public void ToggleBox()
    {
        if (boxAnimator == null)
        {
            Debug.LogWarning("Animator belum diassign!");
            return;
        }

        Debug.Log("ToggleBox dipanggil, isMoved = " + isMoved + ", isOpen = " + isOpen);

        if (!isMoved)
        {
            isMoving = true;
            Debug.Log("Mulai pindah kardus...");
        }
        else
        {
            if (isOpen)
            {
                boxAnimator.SetTrigger("Close");
                isOpen = false;
                Debug.Log("Tutup kardus");
                canDrag = true; // baru boleh drag setelah ditutup
            }
            else
            {
                boxAnimator.SetTrigger("Open");
                isOpen = true;
                Debug.Log("Buka kardus");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EWasteZone"))
        {
            if (boxType == "ewaste")
            {
                Debug.Log("✅ Kardus cocok! Ini e-waste.");
                // Tambahkan feedback (misal efek, sound, dll)
            }
            else
            {
                Debug.Log("❌ Kardus salah. Bukan e-waste.");
                // Tambahkan reaksi jika salah
            }
        }
    }
}
