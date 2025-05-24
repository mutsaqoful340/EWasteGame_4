using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public CameraZoomTransition zoomTransition;


    public Transform target;
    public float speed = 2f;

    private bool isMoving = false;

    public void Update()
    {
        if (!isMoving || target == null) return;

        // Debug log untuk tracking
        Debug.Log($"Moving camera from {transform.position} to {target.position}");

        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
            isMoving = false;
            Debug.Log("Camera reached target position.");
            OnReachedTarget();
        }
    }

    public void StartTransition()
    {
        isMoving = true;
        Debug.Log("Camera transition started.");
    }

    private void OnReachedTarget()
    {
        Debug.Log("Camera reached target. Mini game can start.");
        // Bisa panggil mini game start di sini
    }
}
