using UnityEngine;

public class SpawnerKomponen11 : MonoBehaviour
{
    [Header("Prefab & Parent")]
    public GameObject prefabKomponen;
    public Transform spawnParent;

    [Header("Data Komponen")]
    public Sprite[] gambarKomponen;
    public string[] nama;
    [TextArea]
    public string[] deskripsi;
    public bool[] valid;

    void Start()
    {
        int jumlah = Mathf.Min(nama.Length, deskripsi.Length, gambarKomponen.Length, valid.Length);

        for (int i = 0; i < jumlah; i++)
        {
            GameObject obj = Instantiate(prefabKomponen, spawnParent);

            KomponenItem data = obj.GetComponent<KomponenItem>();
            if (data != null)
            {
                data.Init(nama[i], deskripsi[i], gambarKomponen[i], valid[i]);
            }
            else
            {
                Debug.LogWarning("Prefab tidak memiliki KomponenItem.cs!");
            }
        }
    }
}
