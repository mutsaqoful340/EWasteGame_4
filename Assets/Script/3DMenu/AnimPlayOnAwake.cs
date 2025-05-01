using UnityEngine;

public class SimpleAnimationPlayer : MonoBehaviour
{
    public Animator animator;
    public string animationStateName;

    void Awake()
    {
        if (animator != null && !string.IsNullOrEmpty(animationStateName))
        {
            animator.Play(animationStateName);
        }
        else
        {
            Debug.LogWarning("Animator or Animation State Name is missing!");
        }
    }
}
