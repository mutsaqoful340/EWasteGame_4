using UnityEngine;
using UnityEngine.UI;

public class BlinkOverlay : MonoBehaviour
{
    public float blinkSpeed = 1f; // kecepatan kedap kedip
    private Image image;
    private Color originalColor;

    void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    void Update()
    {
        float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f); // nilai alpha bolak-balik antara 0 dan 1
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}
