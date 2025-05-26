using UnityEngine;
using System.Collections;

public class HPTakerKeyboardBoxAnim : MonoBehaviour
{
    public Transform targetPosition;       // Posisi keluar HP (teleport)
    public GameObject komponenTambahanPrefab; // Spawn prefab tambahan (opsional)
    public Transform spawnPosisi;              // Posisi spawn prefab (opsional)

    public Transform boxTransform;         // Box yang mau dipindah animasi
    public Vector3 boxPosisiBaru;          // Posisi baru Box
    public float durasiAnimasi = 1f;       // Lama animasi perpindahan Box

    [HideInInspector]
    public bool isTaken = false;            // Status sudah diambil

    private HPTakerKeyboardBoxAnim[] allHPsInBox;

    void Start()
    {
        if (boxTransform != null)
        {
            allHPsInBox = boxTransform.GetComponentsInChildren<HPTakerKeyboardBoxAnim>();
        }
    }

    // Dipanggil dari HPDragHandler saat drag selesai
    public void TakeHP()
    {
        if (isTaken) return;

        // Pindah HP langsung
        if (targetPosition != null)
        {
            transform.position = targetPosition.position;
            transform.SetParent(null);
        }

        // Spawn prefab tambahan kalau ada
        if (komponenTambahanPrefab != null && spawnPosisi != null)
        {
            Instantiate(komponenTambahanPrefab, spawnPosisi.position, spawnPosisi.rotation);
        }

        isTaken = true;

        // Cek apakah semua HP sudah diambil
        if (allHPsInBox != null)
        {
            bool semuaSudahDiambil = true;
            foreach (var hp in allHPsInBox)
            {
                if (!hp.isTaken)
                {
                    semuaSudahDiambil = false;
                    break;
                }
            }

            if (semuaSudahDiambil)
            {
                Debug.Log("Semua HP sudah diambil, mulai animasi pindah box");
                if (boxTransform != null)
                {
                    StartCoroutine(MoveOverSeconds(boxTransform, boxPosisiBaru, durasiAnimasi));
                }
            }
            else
            {
                Debug.Log("Masih ada HP yang belum diambil, box belum dipindah");
            }
        }
    }

    IEnumerator MoveOverSeconds(Transform obj, Vector3 targetPos, float seconds)
    {
        Vector3 startPos = obj.position;
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            obj.position = Vector3.Lerp(startPos, targetPos, elapsedTime / seconds);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.position = targetPos; // Pastikan posisi akhir tepat
    }
}
