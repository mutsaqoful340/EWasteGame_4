using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SpawnOnClick : MonoBehaviour
{
    public enum SpawnType { HP, Laptop, PC }
    public SpawnType spawnType = SpawnType.HP;

    [Header("Spawn Settings")]
    public GameObject[] prefabsToSpawn;
    public Transform centerPoint;
    public float radius = 1.0f;
    private bool hasSpawned = false;

    [Header("Laptop Grid Settings")]
    public int laptopColumns = 3;
    public float spacing = 0.5f;

    [Header("Tooltip Settings")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    [Header("Tutorial Panel Settings")]
    public GameObject tutorialPanelPrefab;
    [TextArea]
    public string tutorialText = "Geser dan masukkan komponen ke dalam box sesuai jenisnya.";
    public float tutorialHideDelay = 5f;

    void Start()
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (!hasSpawned)
        {
            if (centerPoint == null) centerPoint = this.transform;

            if (spawnType == SpawnType.HP) SpawnFlatCircle();
            else if (spawnType == SpawnType.Laptop) SpawnLaptopComponents();
            else if (spawnType == SpawnType.PC) SpawnPCComponents();

            ShowTutorialPanel();
            hasSpawned = true;
        }
    }

    void SpawnFlatCircle()
    {
        int total = prefabsToSpawn.Length;
        if (total == 0) return;

        float angleStep = 360f / total;

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];
            if (prefab == null) continue;

            float angleRad = angleStep * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;
            Vector3 spawnPos = centerPoint.position + offset;

            Quaternion flatRotation = Quaternion.Euler(90f, 0f, 0f);
            GameObject obj = Instantiate(prefab, spawnPos, flatRotation);
            SetupTooltip(obj);
        }
    }

    void SpawnLaptopComponents()
    {
        int total = prefabsToSpawn.Length;
        if (total == 0) return;

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];
            if (prefab == null) continue;

            int row = i / laptopColumns;
            int col = i % laptopColumns;

            Vector3 offset = new Vector3(col * spacing, 0, -row * spacing);
            Vector3 spawnPos = centerPoint.position + offset;

            Quaternion flatRotation = Quaternion.Euler(90f, 0f, 0f);
            GameObject obj = Instantiate(prefab, spawnPos, flatRotation);
            SetupTooltip(obj);
        }
    }

    void SpawnPCComponents()
    {
        int total = prefabsToSpawn.Length;
        if (total == 0) return;

        float angleStep = 360f / total;

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];
            if (prefab == null) continue;

            float angleRad = angleStep * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;
            Vector3 spawnPos = centerPoint.position + offset + Vector3.up * 0.01f;

            Quaternion flatRotation = Quaternion.Euler(90f, 0f, 0f);
            GameObject obj = Instantiate(prefab, spawnPos, flatRotation);
            SetupTooltip(obj);
        }
    }

    void SetupTooltip(GameObject obj)
    {
        TooltipTrigger trigger = obj.GetComponent<TooltipTrigger>();
        if (trigger != null)
        {
            trigger.tooltipPanel = tooltipPanel;
            trigger.tooltipText = tooltipText;
        }
    }

    void ShowTutorialPanel()
    {
        if (tutorialPanelPrefab == null) return;

        GameObject panel = Instantiate(tutorialPanelPrefab, FindObjectOfType<Canvas>().transform);
        panel.transform.localPosition = Vector3.zero;

        // Tidak perlu ubah teks, karena sudah diatur di prefab
        Animator animator = panel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("SlideIn");
        }

        StartCoroutine(HideAfterDelay(panel));
    }


    IEnumerator HideAfterDelay(GameObject panel)
    {
        yield return new WaitForSecondsRealtime(tutorialHideDelay);
        Destroy(panel);
    }
}
