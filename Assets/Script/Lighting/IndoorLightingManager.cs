using UnityEngine;
using UnityEngine.Rendering;

public class IndoorLightingManager : MonoBehaviour
{
    [Header("Main Light Settings")]
    public Light mainLight;
    public Color lightColor = new Color(1f, 0.956f, 0.839f);
    public float lightIntensity = 0.8f;

    [Header("Ambient Settings")]
    public Color ambientLightColor = new Color(0.4f, 0.4f, 0.4f);
    public float reflectionIntensity = 1.0f;

    [Header("Realtime GI Options")]
    public bool updateRealtimeGI = true;

    void Start()
    {
        SetupLighting();
    }

    void SetupLighting()
    {
        if (mainLight != null)
        {
            mainLight.color = lightColor;
            mainLight.intensity = lightIntensity;
            mainLight.shadows = LightShadows.Soft;
            mainLight.bounceIntensity = 1.5f;
        }

        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = ambientLightColor;
        RenderSettings.reflectionIntensity = reflectionIntensity;
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;

        if (updateRealtimeGI)
        {
            DynamicGI.UpdateEnvironment(); // Hanya ini yang bisa dipakai di runtime
        }
    }
}
