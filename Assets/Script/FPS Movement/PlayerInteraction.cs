using UnityEngine.InputSystem;
using UnityEngine;
using TMPro; // If you use TextMeshPro
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    public FirstPersonController fpsController; // Your movement script
    public Cinemachine.CinemachineVirtualCamera freeRoamVCam;
    public ItemInteractor itemInteractor; // Your item interaction script
    public TMP_Text itemNameText;
    private GameObject heldItem = null;

    [Header("UI")]
    public TextMeshProUGUI hoverUIText;
    public Animator GPUI_Aniamtor;
    public Animator HPMenuAnimator; // Animator for HP menu

    [Header("Interact GP")]
    public Camera playerCamera;
    public float interactDistance = 1.5f; // Distance to interact with items
    public LayerMask interactLayer; // Layer for interaction
    public float FRCamReturnDelay = 0.5f; // Delay to return to free roam camera after interaction

    //References for other scripts
    [HideInInspector] public RaycastHit currentHit;

    private CharacterController controller;
    private PlayerControls inputActions;
    private GameplayPoint nearbyGP;
    public GameplayPoint DTCheck;

    [HideInInspector] public bool inGPMode = false;
    private bool isHPOpened = false; // For HP menu toggle

    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Player.Enable();
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit currentHit;
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        if (!isHPOpened && !inGPMode && Physics.Raycast(ray, out currentHit, interactDistance, interactLayer))
        {
            GameplayPoint gp = currentHit.collider.GetComponent<GameplayPoint>();
            if (gp != null)
            {
                if (nearbyGP != gp)
                {
                    hoverUIText.text = gp.hoverText;
                    hoverUIText.enabled = true;
                    if (inputActions.Player.Interact.triggered)
                    {
                        gp.ActivateGameplay();
                        hoverUIText.enabled = false;
                    }
                }
            }
        }
        else if (nearbyGP != null)
        {
            hoverUIText.text = nearbyGP.hoverText;
            hoverUIText.enabled = true;

            if (inputActions.Player.Interact.triggered)
            {
                nearbyGP.ActivateGameplay();
                hoverUIText.enabled = false;
            }
        }
        else
        {
            hoverUIText.enabled = false;
        }
        return;
    }

    private void OnEnable()
    {
        inputActions.Player.MenuHP.performed += ctx => OnHPMenuToggle();
        inputActions.Player.Pickup.performed += ctx => OnPickupItem();
    }

    private void OnDisable()
    {
        inputActions.Player.MenuHP.performed -= ctx => OnHPMenuToggle();
        inputActions.Player.Interact.performed -= ctx => OnPickupItem();
    }

    public void SetNearbyGP(GameplayPoint gp)
    {
        nearbyGP = gp;
    }

    public void ClearNearbyGP(GameplayPoint gp)
    {
        if (nearbyGP == gp)
        {
            nearbyGP = null;
        }
    }

    public void EnterGPMode(GameplayPoint gp)
    {
        inGPMode = true;

        freeRoamVCam.Priority = 5;
        itemInteractor.enabled = false; // Disable item interaction in GP mode
        fpsController.enabled = false;
        //GPUI_Aniamtor.Play("");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitGPMode(GameplayPoint gp)
    {
        gp.DeactivateGameplay();
    }

    public IEnumerator CoExitGPMode(GameplayPoint gp)
    {
        freeRoamVCam.Priority = 20;
        yield return new WaitForSeconds(FRCamReturnDelay);
        inGPMode = false;

        //gp.DeactivateGameplay();
        //GPUI_Aniamtor.Play("");

        fpsController.enabled = true;
        itemInteractor.enabled = true; // Re-enable item interaction

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CloseHPMenu()
    {
        if (isHPOpened)
        {
            isHPOpened = false;
            HPMenuAnimator.Play("HPMenu_OUT");
            fpsController.enabled = true; // Re-enable player controls when HP menu is closed
            itemInteractor.enabled = true; // Re-enable item interaction
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnHPMenuToggle()
    {
        // If the menu is about to open AND you’re in GP mode → don’t do anything
        if (!isHPOpened && inGPMode) return; // Don't toggle HP menu if in GP mode
        isHPOpened = !isHPOpened;

        if (isHPOpened)
        {
            HPMenuAnimator.Play("HPMenu_IN");
            fpsController.enabled = false; // Disable player controls when HP menu is open
            itemInteractor.enabled = false; // Disable item interaction when HP menu is open
            itemNameText.enabled = false; // Hide item name text when HP menu is open
            hoverUIText.enabled = false; // Hide hover text when HP menu is open

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }
        else
        {
            HPMenuAnimator.Play("HPMenu_OUT");
            fpsController.enabled = true; // Re-enable player controls when HP menu is closed
            itemInteractor.enabled = true; // Re-enable item interaction when HP menu is closed
            itemNameText.enabled = true; // Show item name text when HP menu is closed
            hoverUIText.enabled = true; // Show hover text when HP menu is closed

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnPickupItem()
    {
        // Handle item pickup logic here
        Debug.Log("Item picked up!");
    }

    private void ShowGPName(string name)
    {
        if (isHPOpened || itemNameText != null)
        {
            hoverUIText.text = name;
        }
        else
        {
            ShowGPName("");
        }

    }

    void LateUpdate()
    {
    }
}
