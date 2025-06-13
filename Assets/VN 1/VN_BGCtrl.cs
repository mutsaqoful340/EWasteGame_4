using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VN_BGCtrl : MonoBehaviour
{
    public bool isSwitched = false;
    public Image background1;
    public Image background2;
    public Animator animator; 

    public void SwitchImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            background2.sprite = sprite;
            animator.SetTrigger("SwBG1");
        }
        else
        {
            background1.sprite = sprite;
            animator.SetTrigger("SwBG2");
        }
        isSwitched = !isSwitched;
    }
    
    public void SetImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            background1.sprite = sprite;
        }
        else
        {
            background2.sprite = sprite;
        }
    }
}
