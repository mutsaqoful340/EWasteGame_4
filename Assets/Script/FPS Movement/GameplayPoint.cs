using UnityEngine;
using System.Collections;

public class GameplayPoint : MonoBehaviour
{
    [Header("Virtual Camera for this GP")]
    public Cinemachine.CinemachineVirtualCamera gpVirtualCamera;

    [Header("Hover UI Text")]
    public string hoverText = "Press E to interact";

    [Header("Gameplay UI")]
    public GameObject gameplayUI;
    public Animator gameplayUIAnimCtrl;

    [Header("Item Position Slots")]
    public Transform itemSlotPoint;

    private bool playerInRange = false;
    public PlayerInteraction playerInteraction;

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

        Debug.Log("Activating GameplayPoint: " + name);

        // Boost this GP VCam priority
        gpVirtualCamera.Priority = 20;

        // Tell player to switch to GP mode
        playerInteraction.EnterGPMode(this);
        gameplayUI.SetActive(true);
        gameplayUIAnimCtrl.Play("CnvGameplay_GPIN");
    }


    // Called when done
    public void DeactivateGameplay()
    {
        Debug.Log("Deactivating GameplayPoint: " + name);

        // Lower this GP VCam priority
        gpVirtualCamera.Priority = 5;
        gameplayUIAnimCtrl.Play("CnvGameplay_GPOUT");
        playerInteraction.StartCoroutine(playerInteraction.CoExitGPMode(this));
    }

    public void ASDASDSAD()
    {
        Debug.Log("ASDASDSAD called for GameplayPoint");
    }
}
