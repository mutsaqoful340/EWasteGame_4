using UnityEngine;
using TMPro;

public class BoxPenyimpanan : MonoBehaviour
{
    public GameplayTimer gameplayTimer;  // Referensi ke GameplayTimer
    public int maxItems = 4;  // Jumlah barang yang harus dimasukkan
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
        if (other.CompareTag("Barang"))
        {
            currentItems++;
            Debug.Log("Item masuk! Total: " + currentItems);

            other.gameObject.SetActive(false);

            if (currentItems >= maxItems)
            {
                Debug.Log("Semua barang sudah dimasukkan, menyelesaikan permainan.");
                gameplayTimer.SelesaikanPermainan();  // Memanggil metode SelesaikanPermainan() dari GameplayTimer
            }
        }
    }
}
