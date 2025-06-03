using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea] public string tooltipInfo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover enter: " + gameObject.name);
        GameTooltipManager.Instance.ShowTooltip(tooltipInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Hover exit: " + gameObject.name);
        GameTooltipManager.Instance.HideTooltip();
    }
}
