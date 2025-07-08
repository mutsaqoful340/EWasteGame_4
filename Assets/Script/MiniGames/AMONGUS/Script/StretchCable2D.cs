using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StretchCable2D : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform cableImage;
    public string correctEndTag = "End_Red"; // Tag tujuan yang benar
    private RectTransform startPoint;

    public GameManagerAmongUs gameManagerAmongUs; // Drag dari Inspector

    private bool connected = false;

    void Start()
    {
        startPoint = GetComponent<RectTransform>();
        cableImage.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (connected) return;
        cableImage.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (connected) return;

        Vector3 mousePos = Input.mousePosition;
        Vector3 startPos = startPoint.position;

        Vector3 direction = mousePos - startPos;
        float distance = direction.magnitude;

        cableImage.position = startPos + direction / 2;
        cableImage.sizeDelta = new Vector2(distance, cableImage.sizeDelta.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        cableImage.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (connected) return;

        GameObject hitObj = GetUIObjectUnderMouse();

        if (hitObj != null && hitObj.CompareTag(correctEndTag))
        {
            Debug.Log("✅ Kabel tersambung!");
            connected = true;

            gameManagerAmongUs.CableConnected(); // ✅ Lapor ke GameManager
        }
        else
        {
            Debug.Log("❌ Salah sambung!");
            cableImage.gameObject.SetActive(false);
        }
    }

    GameObject GetUIObjectUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
            return results[0].gameObject;

        return null;
    }

    public void ResetConnection()
    {
        connected = false;

        if (cableImage != null)
            cableImage.gameObject.SetActive(false);

        // Reset posisi & rotasi kabel ke awal
        cableImage.anchoredPosition = Vector2.zero;
        cableImage.rotation = Quaternion.identity;
        cableImage.sizeDelta = new Vector2(0f, cableImage.sizeDelta.y);
    }
}
