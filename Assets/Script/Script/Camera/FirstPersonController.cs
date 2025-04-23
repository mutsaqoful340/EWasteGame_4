using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float movementSpeed = 3f;
    public float lookSpeedX = 2f;
    public float lookSpeedY = 2f;

    private Camera playerCamera;
    private float rotationX = 0f;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked; // Menyembunyikan cursor dan mengunci di tengah
        Cursor.visible = false;
    }

    void Update()
    {
        // Menggerakkan karakter dengan input WASD
        float moveX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(moveX, 0, moveZ);

        // Menggerakkan kamera berdasarkan gerakan mouse
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);  // Mengatur batas rotasi vertikal agar tidak terbalik
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        float rotationY = Input.GetAxis("Mouse X") * lookSpeedX;
        transform.Rotate(0, rotationY, 0); // Rotasi karakter berdasarkan gerakan mouse horizontal
    }
}
