using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCam_Changer : MonoBehaviour
{
    [Header("Virtual Camera")]
    public Cinemachine.CinemachineVirtualCamera currentVCam;


    public void VCam_IN()
    {
        //Cinemachine.CinemachineVirtualCamera.activeCamera.Priority = 5;
        currentVCam.Priority = 20;
    }

    public void VCam_OUT()
    {
        currentVCam.Priority = 5;
    }
}
