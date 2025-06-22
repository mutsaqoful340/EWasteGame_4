using UnityEngine;

public class GameManager12 : MonoBehaviour
{
    // Singleton Instance
    public static GameManager12 Instance { get; private set; }

    // Data yang ingin disimpan
    public int currentReward = 0;
    public int pelanggaranMakan = 0;
    public int pelanggaranNabung = 0;

    private void Awake()
    {
        // Cek dan setup Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Jangan hancurkan saat pindah scene
        }
        else
        {
            Destroy(gameObject); // Hancurkan duplikat
        }
    }

    private void Start()
    {
        // Debug log untuk cek awal nilai
        Debug.Log($"GameManager Start -> Uang: {currentReward}, Makan: {pelanggaranMakan}, Nabung: {pelanggaranNabung}");
    }

    // Contoh method kalau mau update reward
    public void TambahReward(int jumlah)
    {
        currentReward += jumlah;
        Debug.Log($"Reward ditambah: {jumlah}, Total: {currentReward}");
    }

    // Contoh method kalau mau update pelanggaran makan
    public void TambahPelanggaranMakan()
    {
        pelanggaranMakan++;
        Debug.Log($"Pelanggaran Makan: {pelanggaranMakan}");
    }

    // Contoh method kalau mau update pelanggaran nabung
    public void TambahPelanggaranNabung()
    {
        pelanggaranNabung++;
        Debug.Log($"Pelanggaran Nabung: {pelanggaranNabung}");
    }
}
