using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu3D : MonoBehaviour
{
    public float rotationAmount = 15f; // How much to rotate in degrees
    public float rotationSpeed = 5f;    // How fast it rotates

    private Quaternion targetRotation;
    private Animator animator; // Reference to Animator

    void Start()
    {
        targetRotation = transform.rotation; // Set initial rotation
        animator = GetComponent<Animator>(); // Get Animator component
    }

    void Update()
    {
        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void RotateCameraRight()
    {
        targetRotation *= Quaternion.Euler(0, rotationAmount, 0);
    }

    public void RotateCameraLeft()
    {
        targetRotation *= Quaternion.Euler(0, -rotationAmount, 0);
    }

    public void RotateCameraUp()
    {
        targetRotation *= Quaternion.Euler(-rotationAmount, 0, 0);
    }

    public void RotateCameraDown()
    {
        targetRotation *= Quaternion.Euler(rotationAmount, 0, 0);
    }

    public void PlayCameraAnimation(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning("No Animator found on the Camera!");
        }
    }
}
