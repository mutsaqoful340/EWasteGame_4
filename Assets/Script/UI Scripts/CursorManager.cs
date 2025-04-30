using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    public Texture2D defaultCursor;
    public Texture2D grabCursor;
    public Texture2D grabableCursor;
    public Vector2 hotspot = Vector2.zero;

    public bool IsDragging { get; private set; } = false; // Global drag flag

    private void Awake()
    {
        // Singleton pattern so you can access it from anywhere
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDefaultCursor()
    {
        if (IsDragging) return; // Don't change cursor during drag
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }

    public void SetGrabCursor()
    {
        Cursor.SetCursor(grabCursor, hotspot, CursorMode.Auto);
    }

    public void SetGrabableCursor()
    {
        if (IsDragging) return; // Don't change cursor during drag
        Cursor.SetCursor(grabableCursor, hotspot, CursorMode.Auto);
    }

    public void StartDragging()
    {
        IsDragging = true;
        SetGrabCursor();
    }

    public void StopDragging()
    {
        IsDragging = false;
    }
}
