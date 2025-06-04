using UnityEngine;

public class UIAnim_Script : MonoBehaviour
{
    Audio_Manager audioManager;
    public Animator animator;

    private bool isShowing = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isShowing)
            {
                animator.Play("GP_LaptopIN");
            }
            else
            {
                animator.Play("GP_LaptopOUT");
            }

            isShowing = !isShowing;
        }
    }

}

