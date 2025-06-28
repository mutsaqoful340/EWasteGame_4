using UnityEngine;

public class BoxLidAnimatorController : MonoBehaviour
{
    public Animator boxAnimator;

    private bool isOpen = false;

    public void ToggleBox()
    {
        if (boxAnimator == null) return;

        if (isOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        isOpen = true;
        boxAnimator.speed = 1f;
        boxAnimator.Play("Open", 0, 0f);
    }

    public void Close()
    {
        isOpen = false;
        boxAnimator.speed = 1f;
        boxAnimator.Play("Close", 0, 0f);
    }

    private void OnMouseDown()
    {
        ToggleBox();
    }
}
