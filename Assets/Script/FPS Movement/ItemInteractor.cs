using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ItemInteractor : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float pickupDistance = 1.5f;
    public LayerMask pickupLayer;
    public Transform holdPoint;
    public Camera playerCamera;
    public LayerMask interactLayer;

    [Header("UI")]
    public TMP_Text itemNameText;

    [Header("Item Properties")]
    public float setScale;


    private GameplayPoint focusedGP = null;
    private PickableItem focusedItem = null;
    private bool isHPOpened = false; // For HP menu toggle

    [HideInInspector] public PickableItem heldItem = null;
    [HideInInspector] public Rigidbody heldRb;

    private PlayerControls inputActions;

    private void Awake()
    {
        inputActions = new PlayerControls();

        inputActions.Player.Interact.performed += _ => TryPickupOrDrop();
        inputActions.Player.Drop.performed += _ => DropItem();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * pickupDistance, Color.red);

        if (heldItem == null && Physics.Raycast(ray, out hit, pickupDistance, pickupLayer))
        {
            PickableItem item = hit.collider.GetComponent<PickableItem>();

            if (item != null && !item.isHeld)
            {
                if (focusedItem != item)
                {
                    ClearHighlight();
                    focusedItem = item;
                    focusedItem.Highlight(true);
                    ShowItemName(focusedItem.itemName);
                }
                return;
            }
        }

        ClearHighlight();
    }

    private void TryPickupOrDrop()
    {
        if (heldItem == null && focusedItem != null)
            PickupItem(focusedItem);
        else if (heldItem != null)
            DropItem();
    }

    private void PickupItem(PickableItem item)
    {
        heldItem = item;
        heldRb = heldItem.GetComponent<Rigidbody>();

        heldItem.originalScale = heldItem.transform.localScale; // Store original scale
        heldItem.transform.localScale = Vector3.one * setScale; // Example: half size

        heldItem.isHeld = true;
        heldItem.Highlight(false);
        heldItem.transform.SetParent(holdPoint);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;

        heldRb.useGravity = false;
        heldRb.isKinematic = true;

        ClearHighlight();
    }

    private void DropItem()
    {
        if (heldItem == null) return;

        heldItem.transform.SetParent(null);
        heldRb.useGravity = true;
        heldRb.isKinematic = false;
        heldItem.isHeld = false;
        heldItem.transform.localScale = heldItem.originalScale;

        heldItem = null;
        heldRb = null;
    }

    private void ClearHighlight()
    {
        if (focusedItem != null)
        {
            focusedItem.Highlight(false);
            focusedItem = null;
            ShowItemName("");
        }
    }

    private void ShowItemName(string name)
    {
        if (isHPOpened || itemNameText != null)
        {
            itemNameText.text = name;
        }
        else
        {
            ShowItemName("");
        }

    }
}
