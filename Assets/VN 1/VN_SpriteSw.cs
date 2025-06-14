using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VN_SpriteSw : MonoBehaviour
{
    public bool isSwitched = false;
    public Image img1;
    public Image img2;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            img2.sprite = sprite;
            animator.SetTrigger("SwBG1");
        }
        else
        {
            img1.sprite = sprite;
            animator.SetTrigger("SwBG2");
        }
        isSwitched = !isSwitched;
    }

    public void SetImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            img1.sprite = sprite;
        }
        else
        {
            img2.sprite = sprite;
        }
    }

    public Sprite GetImage()
    {
        if (!isSwitched)
        {
            return img1.sprite;
        }
        else
        {
            return img2.sprite;
        }

    }

}
