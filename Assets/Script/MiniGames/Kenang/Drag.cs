using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler11 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 startPosition;
    private Transform parentToReturnTo;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        parentToReturnTo = transform.parent;

        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root); // keluar dari layout agar bebas gerak
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentToReturnTo);
        transform.position = startPosition;
    }
}
