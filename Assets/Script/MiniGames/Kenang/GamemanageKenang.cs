using UnityEngine;

public class GameManagerKenang : MonoBehaviour
{
    public static GameManagerKenang instance;

    public int totalItem = 10;
    private int currentDrop = 0;

    public GameObject panelAkhir;
    public bool sudahSelesai = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (panelAkhir != null)
            panelAkhir.SetActive(false);
    }

    public void TambahItemMasuk()
    {
        currentDrop++;
        Debug.Log("Item masuk: " + currentDrop + "/" + totalItem);

        if (currentDrop >= totalItem)
        {
            Debug.Log("Semua item sudah masuk. Tunggu panel info ditutup.");
            sudahSelesai = true;
        }
    }

    public void TampilkanPanelAkhir()
    {
        if (panelAkhir != null)
            panelAkhir.SetActive(true);
    }
}
