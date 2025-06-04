using UnityEngine;

public class UIAnim_Script : MonoBehaviour
{
    Audio_Manager audioManager;
    public Animator animator;
    public string triggerIn = "IN";   // Set this to your IN trigger name
    public string triggerOut = "OUT"; // Set this to your OUT trigger name

    private bool isShowing = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isShowing)
            {
                animator.SetTrigger(triggerOut);
            }
            else
            {
                animator.SetTrigger(triggerIn);
            }

            isShowing = !isShowing;
        }
    }

}

