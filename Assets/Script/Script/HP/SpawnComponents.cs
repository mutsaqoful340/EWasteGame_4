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
    public float radius = 1.5f;
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

            ShufflePrefabs(); // 🔀 Acak urutan prefab

            if (spawnType == SpawnType.HP) SpawnFlatCircle();
            else if (spawnType == SpawnType.Laptop) SpawnLaptopComponents();
            else if (spawnType == SpawnType.PC) SpawnPCComponents();

            ShowTutorialPanel();
            hasSpawned = true;

            // Sembunyikan objek utama
            gameObject.SetActive(false);
        }
    }

    void SpawnFlatCircle()
    {
        int total = prefabsToSpawn.Length;
        if (total == 0) return;

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];
            if (prefab == null) continue;

            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 spawnPos = centerPoint.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            Quaternion randomRotation = Quaternion.Euler(
                90f,
                Random.Range(0f, 360f),
                0f
            );

            GameObject obj = Instantiate(prefab, spawnPos, randomRotation);

            float scaleFactor = Random.Range(0.9f, 1.1f);
            obj.transform.localScale *= scaleFactor;

            SetupTooltip(obj);
        }
    }

    void SpawnLaptopComponents()
    {
        int total = prefabsToSpawn.Length;
        if (total == 0) return;

        int rows = Mathf.CeilToInt((float)total / laptopColumns);
        float halfWidth = (laptopColumns - 1) * spacing * 0.5f;
        float halfHeight = (rows - 1) * spacing * 0.5f;

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];
            if (prefab == null) continue;

            int row = i / laptopColumns;
            int col = i % laptopColumns;

            Vector3 baseOffset = new Vector3(
                (col * spacing) - halfWidth,
                0,
                -(row * spacing) + halfHeight
            );

            // 🔄 Acak tapi masih dekat, tidak jauh dari grid
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.1f, 0.1f),
                0f,
                Random.Range(-0.1f, 0.1f)
            );

            Vector3 spawnPos = centerPoint.position + baseOffset + randomOffset;

            Quaternion rotation = Quaternion.Euler(90f, Random.Range(-5f, 5f), 0f); // Biar gak terlalu flat
            GameObject obj = Instantiate(prefab, spawnPos, rotation);

            float scaleFactor = Random.Range(0.97f, 1.03f);
            obj.transform.localScale *= scaleFactor;

            SetupTooltip(obj);
        }
    }





    void SpawnPCComponents()
    {
        int total = prefabsToSpawn.Length;
        if (total == 0) return;

        for (int i = 0; i < total; i++)
        {
            GameObject prefab = prefabsToSpawn[i];
            if (prefab == null) continue;

            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 spawnPos = centerPoint.position + new Vector3(randomCircle.x, 0, randomCircle.y) + Vector3.up * 0.01f;

            // Pakai rotasi 90 derajat agar datar
            Quaternion flatRotation = Quaternion.Euler(0f, 0f, 90f);

            GameObject obj = Instantiate(prefab, spawnPos, flatRotation);

            // Paksa transform lokal anak-anak ikut rata juga
            obj.transform.rotation = flatRotation;

            // Reset rotasi lokal jika prefab punya anak2 yg miring
            foreach (Transform child in obj.transform)
            {
                child.localRotation = Quaternion.identity;
            }

            float scaleFactor = Random.Range(0.9f, 1.1f);
            obj.transform.localScale *= scaleFactor;

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

    void ShufflePrefabs()
    {
        for (int i = 0; i < prefabsToSpawn.Length; i++)
        {
            int randIndex = Random.Range(i, prefabsToSpawn.Length);
            GameObject temp = prefabsToSpawn[i];
            prefabsToSpawn[i] = prefabsToSpawn[randIndex];
            prefabsToSpawn[randIndex] = temp;
        }
    }
}
