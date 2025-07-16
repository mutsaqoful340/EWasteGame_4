using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCam_Changer : MonoBehaviour
{
    [Header("Virtual Camera")]
    public Cinemachine.CinemachineVirtualCamera currentVCam;
    public float VCamDelay = 0.5f; // Delay before switching VCam


    public void VCam_IN()
    {
        //Cinemachine.CinemachineVirtualCamera.activeCamera.Priority = 5;
        currentVCam.Priority = 20;
    }

    public void VCam_OUT()
    {
        //Cinemachine.CinemachineVirtualCamera.activeCamera.Priority = 5;
        currentVCam.Priority = 5;
    }
    public void GPVCam_OUT(GameplayPoint gp)
    {
        gp.DeactivateGameplay();
    }


    private IEnumerator Co_VCam_OUT(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentVCam.Priority = 5;
    }
}
