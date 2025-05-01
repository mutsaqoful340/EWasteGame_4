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
        Debug.Log("==> Masuk collider: " + other.name + " | Tag: " + other.tag);

        if (other.CompareTag("BARANG"))
        {
            currentItems++;
            Debug.Log("Item BARANG masuk! Total: " + currentItems);

            other.gameObject.SetActive(false);

            if (currentItems >= maxItems)
            {
                Debug.Log("Semua barang sudah dimasukkan. Menyelesaikan permainan.");
                gameplayTimer.SelesaikanPermainan();
            }
        }
        else if (other.CompareTag("EWASTE"))
        {
            Debug.Log("Item dengan tag 'EWASTE' tidak boleh masuk ke BoxPenyimpanan.");

            ItemPositionReset resetScript = other.GetComponent<ItemPositionReset>();
            if (resetScript != null)
            {
                resetScript.ResetPosition();
                Debug.Log("Posisi item 'EWASTE' dikembalikan.");
            }
            else
            {
                Debug.LogWarning("Item 'EWASTE' tidak memiliki komponen ItemPositionReset.");
            }
        }
        else
        {
            Debug.Log("Tag tidak dikenali, tidak diproses: " + other.tag);
        }
    }
}
