using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VN_SpriteCtrl : MonoBehaviour
{
    private VN_SpriteSw switcher;
    private Animator animator;
    private RectTransform rect;

    private void Awake()
    {
        switcher = GetComponent<VN_SpriteSw>();
        animator = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
    }

    public void Setup(Sprite sprite)
    {
        switcher.SetImage(sprite);
    }

    public void Show(Vector2 coords)
    {
        animator.SetTrigger("CharShow");
        rect.localPosition = coords;
    }

    public void Hide(Vector2 coords)
    {
        animator.SetTrigger("CharHide");
        rect.localPosition = coords;
    }

    public void Move(Vector2 coords, float speed)
    {
        StartCoroutine(MoveCoroutine(coords, speed));
    }

    private IEnumerator MoveCoroutine(Vector2 coords, float speed)
    {
        while(rect.localPosition.x != coords.x || rect.localPosition.y != coords.y)
        {
            rect.localPosition = Vector2.MoveTowards(rect.localPosition, coords, Time.deltaTime * 1000f * speed);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SwitchSprite(Sprite sprite)
    {
        if(switcher.GetImage() != sprite)
        {
            switcher.SwitchImage(sprite);
        }
    }
}
