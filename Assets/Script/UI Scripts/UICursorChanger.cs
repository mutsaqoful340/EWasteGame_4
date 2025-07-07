using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UICursorChanger : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler, IPointerUpHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetGrabableCursor();
        Debug.Log("Pointer entered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetDefaultCursor();
        Debug.Log("Pointer exited");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CursorManager.Instance.StopDragging();
        Debug.Log("Pointer up");
    }
}
