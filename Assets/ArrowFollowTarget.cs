// ArrowFollowTarget.cs
using UnityEngine;

public class ArrowFollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0); // posisi panah di atas objek

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
