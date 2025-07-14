using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class PickableItem : MonoBehaviour
{
    [Header("Item Properties")]
    [HideInInspector] public Vector3 originalScale;
    public string itemName = "Item";

    [HideInInspector] public bool isHeld = false;

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;
    }

    public void Highlight(bool on)
    {
        if (outline != null && !isHeld)
            outline.enabled = on;
    }
}
