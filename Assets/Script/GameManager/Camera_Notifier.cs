using UnityEngine;

[RequireComponent(typeof(Camera))]  // Ensures this is only attached to Camera objects
public class Camera_Notifier : MonoBehaviour
{
    private void OnEnable()
    {
        // Only notify if this is the active MainCamera
        if (gameObject.CompareTag("MainCamera"))
        {
            Camera_Manager.Instance?.UpdateCameraReference();
        }
    }

    // Optional: Handle camera disable/switch
    private void OnDisable()
    {
        if (Camera.main == null)
            Debug.Log("Camera disabled: " + gameObject.name);
    }
}