using UnityEngine;
using TMPro;

public class GameTooltipManager : MonoBehaviour
{
    public static GameTooltipManager Instance;

    [Header("Assign in Inspector")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    [Header("Settings")]
    public Vector3 offset = new Vector3(15f, -15f, 0f);

    private RectTransform tooltipRect;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (tooltipPanel == null || tooltipText == null)
        {
            Debug.LogError("TooltipPanel or TooltipText not assigned in Inspector!");
            return;
        }

        tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        HideTooltipImmediate();
    }

    private void Update()
    {
        if (tooltipPanel.activeSelf)
            UpdateTooltipPosition();
    }

    private void UpdateTooltipPosition()
    {
        Vector3 pos = Input.mousePosition + offset;

        Vector2 size = tooltipRect.sizeDelta;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (pos.x + size.x > screenWidth)
            pos.x = screenWidth - size.x;

        if (pos.y - size.y < 0)
            pos.y = size.y;

        tooltipRect.position = pos;

        Debug.Log($"Tooltip position updated: {pos}");
    }

    public void ShowTooltip(string message)
    {
        if (tooltipPanel == null || tooltipText == null)
            return;

        tooltipText.text = message;
        tooltipPanel.SetActive(true);
        tooltipPanel.transform.position = Input.mousePosition + offset;

        Debug.Log($"ShowTooltip called with message: {message}");
    }

    public void HideTooltip()
    {
        if (tooltipPanel == null)
            return;

        tooltipPanel.SetActive(false);
        Debug.Log("HideTooltip called");
    }

    private void HideTooltipImmediate()
    {
        if (tooltipPanel == null)
            return;

        tooltipPanel.SetActive(false);
    }
}
