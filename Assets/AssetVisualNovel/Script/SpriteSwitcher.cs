using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    public bool isSwitched = false;
    public Image Image1;
    public Image Image2;
    private Animator animator;

    private void awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            Image2.sprite = sprite;
            animator.SetTrigger("SwitchFirst");
        }
        else
        {
            Image1.sprite = sprite;
            animator.SetTrigger("SwitchSecond");
        }
        isSwitched = !isSwitched;
    }

    public void SetImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            Image1.sprite = sprite;
        }
        else
        {
            Image2.sprite = sprite;
        }
    }


        public Sprite GetImage()
        {
            if (!isSwitched)
            {
                return Image1.sprite;
            }
            else
            {
                return Image2.sprite;
            }
        }
    }


