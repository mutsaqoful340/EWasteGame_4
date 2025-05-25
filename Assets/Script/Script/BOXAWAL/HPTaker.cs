using UnityEngine;
using System.Collections;

public class HPTakerKeyboardBoxAnim : MonoBehaviour
{
    public Transform targetPosition;       // Posisi keluar HP (langsung teleport)
    public GameObject komponenTambahanPrefab; // Spawn prefab (optional)
    public Transform spawnPosisi;              // Posisi spawn prefab (optional)

    public Transform boxTransform;         // Box yang mau dipindah animasi
    public Vector3 boxPosisiBaru;          // Posisi baru Box

    public float durasiAnimasi = 1f;       // Lama animasi perpindahan Box

    private bool isTaken = false;

    void Update()
    {
        if (!isTaken && Input.GetKeyDown(KeyCode.E))
        {
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

            // Mulai animasi perpindahan Box
            if (boxTransform != null)
            {
                StartCoroutine(MoveOverSeconds(boxTransform, boxPosisiBaru, durasiAnimasi));
            }

            isTaken = true;
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
