using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomTutorial : MonoBehaviour
{
    public ItemInteractor itemInteractor; // Reference to PlayerInteraction script
    public FirstPersonController fpsController; // Reference to FirstPersonController script
    //public GameObject[] DTObjects; // Array of tutorial objects to activate
    public PlayerInteraction playerInteraction; // Reference to PlayerInteraction script

    public bool DTActive = false; // Flag to check if Doom Tutorial is active
    //private int currentDTIndex = 0; // Current index of the tutorial object
    private PlayerControls inputActions;
    public bool notDay1 = false; // Flag to check if it's not Day 1

    public void Start()
    {
        inputActions = new PlayerControls();
        inputActions.Player.Enable();

        //DTActive = true; // Reset the flag when starting the tutorial

        if (!DTActive)
        {
            itemInteractor.enabled = true; // Disable item interaction in GP mode
            fpsController.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
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
    
    public void DTOpen()
    {
        if (!DTActive) //&& currentDTIndex < DTObjects.Length && DTObjects[currentDTIndex] != null)
        {
            DTActive = true;

            //DTObjects[currentDTIndex].SetActive(true);
            //Debug.Log("Doom Tutorial activated: " + DTObjects[currentDTIndex].name);
        }
        else
        {
            //Debug.LogWarning("Doom Tutorial is already active or no more objects to activate.");
        }
    }

    public void DTClose()
    {
        DTActive = false;
        //DTObjects[currentDTIndex].SetActive(false);
        //currentDTIndex++;
        itemInteractor.enabled = true; // Disable item interaction in GP mode
        fpsController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
