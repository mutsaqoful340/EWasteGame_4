using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UICursorChanger : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler, IPointerUpHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetGrabableCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetDefaultCursor();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CursorManager.Instance.StopDragging();
    }
}
