using UnityEngine;
using UnityEngine.UI;

public class BottleTutorial : MonoBehaviour
{
    public GameObject tutorialPanel; // Panel yang berisi instruksi
    public string tagBotol = "Botol"; // Pastikan objek botol punya tag ini

    private bool hasShownTutorial = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasShownTutorial)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag(tagBotol))
                {
                    ShowTutorial();
                }
            }
        }
    }

    void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        hasShownTutorial = true;
        Invoke("HideTutorial", 5f); // Hilangkan panel setelah 5 detik
    }

    void HideTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}
