using UnityEngine;

public class ItemPositionReset : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        GetComponent<Rigidbody>().velocity = Vector3.zero; // reset physics
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
