using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    public Texture2D defaultCursor;
    public Texture2D grabCursor;
    public Vector2 hotspot = Vector2.zero;

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
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }

    public void SetGrabCursor()
    {
        Cursor.SetCursor(grabCursor, hotspot, CursorMode.Auto);
    }
}
