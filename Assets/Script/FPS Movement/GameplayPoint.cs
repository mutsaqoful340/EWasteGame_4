using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayPoint : MonoBehaviour
{
    private static Dictionary<string, int> clickCountsDT = new Dictionary<string, int>();

    [Header("Virtual Camera for this GP")]
    public Cinemachine.CinemachineVirtualCamera gpVirtualCamera;

    [Header("Hover UI Text")]
    public string hoverText = "Press E to interact";

    [Header("Gameplay UI")]
    public GameObject gameplayUI;
    public Animator gameplayUIAnimCtrl;

    [Header("Doom Tutorial")]
    public DoomTutorial doomTutorial; // Reference to DoomTutorial script
    public GameObject DTPanel;
    public Animator DTAnimCtrl;
    public bool DTActive = false; // Flag to check if Doom Tutorial is active
    public string dtID;
    private bool DThasShown = false;
    public Collider colliderGP;

    private bool playerInRange = false;
    private PlayerControls inputActions;
    [HideInInspector] public ItemInteractor itemInteractor;
    [HideInInspector] public FirstPersonController fpsController;
    [HideInInspector] public PlayerInteraction playerInteraction;
    [HideInInspector] public bool inGPMode = false;
    public bool ingoreDT = false; // Ignore Doom Tutorial flag

    private void Start()
    {
        inputActions = new PlayerControls();
        inputActions.Player.Enable();
        colliderGP = GetComponent<Collider>();

        DTActive = true; // Reset the flag when starting the tutorial

        if (!DTActive && !ingoreDT)
        {
            itemInteractor.enabled = true; // Disable item interaction in GP mode
            fpsController.enabled = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }
        else
        {
            itemInteractor.enabled = false; // Disable item interaction in GP mode
            fpsController.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.SetNearbyGP(this);
                playerInRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerInRange)
        {
            if (playerInteraction != null)
            {
                playerInteraction.ClearNearbyGP(this);
                playerInteraction = null;
            }
            playerInRange = false;
        }
    }

    // Called by PlayerInteraction when player presses E
    public void ActivateGameplay()
    {
        DTActive = true; // Set Doom Tutorial active
        if (!DThasShown && !ingoreDT)
        {
            DTPanel.SetActive(true);
        }

        // Boost this GP VCam priority
        gpVirtualCamera.Priority = 20;

        // Tell player to switch to GP mode
        playerInteraction.EnterGPMode(this);
        gameplayUI.SetActive(true);
        gameplayUIAnimCtrl.Play("CnvGameplay_GPIN");
        colliderGP.enabled = false; // Disable collider to prevent re-triggering
    }


    // Called when done
    public void DeactivateGameplay()
    {
        Debug.Log("Deactivating GameplayPoint: " + name);

        // Lower this GP VCam priority
        gpVirtualCamera.Priority = 5;
        gameplayUIAnimCtrl.Play("CnvGameplay_GPOUT");
        playerInteraction.StartCoroutine(playerInteraction.CoExitGPMode(this));
        DTAnimCtrl.Play("DT2_1_OUT");
        colliderGP.enabled = true; // Re-enable collider for future interactions
    }


}
