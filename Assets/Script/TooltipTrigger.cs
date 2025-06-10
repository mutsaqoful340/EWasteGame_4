using UnityEngine;
using TMPro;

public class TooltipTrigger : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;
    [TextArea]
    public string infoText;

    private bool isTooltipVisible = false;

    private void OnMouseDown()
    {
        if (tooltipPanel != null && tooltipText != null)
        {
            isTooltipVisible = !isTooltipVisible;

            tooltipPanel.SetActive(isTooltipVisible);
            if (isTooltipVisible)
            {
                tooltipText.text = infoText;
            }
        }
        else
        {
            Debug.LogWarning("Tooltip Panel atau Text belum di-assign!");
        }
    }

    private void OnMouseExit()
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
            isTooltipVisible = false;
        }
    }
}
