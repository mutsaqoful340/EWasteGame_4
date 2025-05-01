using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera mainCamera; // Kamera utama untuk bergerak
    public Transform tvPosition; // Posisi TV untuk kamera utama bergerak ke sana
    public float moveSpeed = 5f; // Kecepatan pergerakan kamera utama

    void Update()
    {
        // Pindahkan kamera utama menuju posisi TV
        if (mainCamera != null && tvPosition != null)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, tvPosition.position, moveSpeed * Time.deltaTime);
        }
    }
}
