using UnityEngine.InputSystem;
using UnityEngine;
using TMPro; // If you use TextMeshPro

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

    private CharacterController controller;
    private PlayerControls inputActions;
    private GameplayPoint nearbyGP;

    private bool inGPMode = false;
    private bool isHPOpened = false; // For HP menu toggle

    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.Player.Enable();
    }

    void Update()
    {
        if (inGPMode) return;

        if (nearbyGP != null)
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
        GPUI_Aniamtor.Play("");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitGPMode(GameplayPoint gp)
    {
        inGPMode = false;

        freeRoamVCam.Priority = 20;
        gp.DeactivateGameplay();
        GPUI_Aniamtor.Play("");

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

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }
        else
        {
            HPMenuAnimator.Play("HPMenu_OUT");
            fpsController.enabled = true; // Re-enable player controls when HP menu is closed
            itemInteractor.enabled = true; // Re-enable item interaction when HP menu is closed
            itemNameText.enabled = true; // Show item name text when HP menu is closed

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnPickupItem()
    {
        // Handle item pickup logic here
        Debug.Log("Item picked up!");
    }

    void LateUpdate()
    {
    }
}
