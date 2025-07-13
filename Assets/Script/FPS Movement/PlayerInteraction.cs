using UnityEngine;
using TMPro; // If you use TextMeshPro

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    public FirstPersonController fpsController; // Your movement script
    public Cinemachine.CinemachineVirtualCamera freeRoamVCam;

    [Header("UI")]
    public TextMeshProUGUI hoverUIText;

    private GameplayPoint nearbyGP;
    private bool inGPMode = false;

    void Update()
    {
        if (inGPMode) return;

        if (nearbyGP != null)
        {
            hoverUIText.text = nearbyGP.hoverText;
            hoverUIText.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                nearbyGP.ActivateGameplay();
            }
        }
        else
        {
            hoverUIText.enabled = false;
        }
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
        fpsController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitGPMode(GameplayPoint gp)
    {
        inGPMode = false;

        // Boost Free Roam VCam priority
        freeRoamVCam.Priority = 20;

        // Deactivate GP
        gp.DeactivateGameplay();

        // Re-enable movement
        fpsController.enabled = true;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (inGPMode && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGPMode(nearbyGP);
        }
    }
}
