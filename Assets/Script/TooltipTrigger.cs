using UnityEngine;

public class TooltipTrigger : MonoBehaviour
{
    public enum TooltipType { DaurUlang, Dijual }
    public TooltipType tooltipType;
    private Camera cam;

    [HideInInspector] public GameObject tooltipPanelDaurUlang;
    [HideInInspector] public GameObject tooltipPanelDijual;

    private bool isTooltipVisible = false;

    void Start()
    {
        cam = Camera.main;
        Ray downRay = new Ray(transform.position + Vector3.up * 2f, Vector3.down);
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                if (!isTooltipVisible)
                {
                    ShowTooltip();
                }
            }
            else
            {
                if (isTooltipVisible)
                {
                    HideAllTooltips();
                }
            }
        }
    }
    
    private void OnMouseDown()
    {
        ShowTooltip();
    }

    private void OnMouseUp()
    {
        HideAllTooltips();
    }

    private void OnMouseExit()
    {
        HideAllTooltips();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hide tooltip jika masuk ke salah satu box
        if (other.CompareTag("StorageZone") || other.CompareTag("TrashZone"))
        {
            HideAllTooltips();
        }
    }

    void ShowTooltip()
    {
        HideAllTooltips(); // Hide yang lain dulu
        isTooltipVisible = true;

        if (tooltipType == TooltipType.DaurUlang && tooltipPanelDaurUlang != null)
            tooltipPanelDaurUlang.SetActive(true);
        else if (tooltipType == TooltipType.Dijual && tooltipPanelDijual != null)
            tooltipPanelDijual.SetActive(true);
    }

    void HideAllTooltips()
    {
        if (tooltipPanelDaurUlang != null)
            tooltipPanelDaurUlang.SetActive(false);

        if (tooltipPanelDijual != null)
            tooltipPanelDijual.SetActive(false);

        isTooltipVisible = false;
    }
}
