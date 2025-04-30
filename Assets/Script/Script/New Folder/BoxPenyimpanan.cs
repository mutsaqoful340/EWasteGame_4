using UnityEngine;
using TMPro;

public class BoxPenyimpanan : MonoBehaviour
{
    public GameplayTimer gameplayTimer;
    public int maxItems = 4;
    private int currentItems = 0;

    public GameObject winPanel;
    public TextMeshProUGUI winText;
    public GameObject buttonsPanel;

    void Start()
    {
        winPanel.SetActive(false);
        buttonsPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Masuk collider dengan tag: " + other.tag);  // Debug untuk melihat tag yang terbaca

        if (other.CompareTag("Ewaste"))
        {
            Debug.Log("Item dengan tag 'Ewaste' tidak boleh masuk ke BoxPenyimpanan.");
            return; // Langsung keluar jika Ewaste
        }

        if (other.CompareTag("Barang"))
        {
            currentItems++;
            Debug.Log("Item Barang masuk! Total: " + currentItems);

            other.gameObject.SetActive(false);

            if (currentItems >= maxItems)
            {
                Debug.Log("Semua barang sudah dimasukkan. Menyelesaikan permainan.");
                gameplayTimer.SelesaikanPermainan();
            }
        }
        else
        {
            Debug.Log("Tag tidak dikenali, tidak diproses: " + other.tag);
        }
    }
}
