using UnityEngine;
using UnityEngine.EventSystems;

public class CableHeadDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CableManager cableManager;
    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Bisa tambahkan efek atau reset manual jika mau
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out localPoint
        );

        // Pakai fungsi yang aman dan ringan
        cableManager.SafeDrawCable(localPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cableManager.TryConnect();
    }
}
