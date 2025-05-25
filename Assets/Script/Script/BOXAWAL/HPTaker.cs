using UnityEngine;

public class HPTakerKeyboard : MonoBehaviour
{
    public Transform targetPosition;           // Tempat HP mau dipindah
    public GameObject komponenTambahanPrefab; // Prefab yang mau di-spawn (optional)
    public Transform spawnPosisi;              // Posisi spawn prefab (optional)
    private bool isTaken = false;

    void Update()
    {
        if (!isTaken && Input.GetKeyDown(KeyCode.E))
        {
            if (targetPosition != null)
            {
                transform.position = targetPosition.position;
                transform.SetParent(null);  // Lepas dari Box supaya HP jadi root di Hierarchy
            }

            if (komponenTambahanPrefab != null && spawnPosisi != null)
            {
                Instantiate(komponenTambahanPrefab, spawnPosisi.position, spawnPosisi.rotation);
            }

            isTaken = true;
            // Jangan ubah layer ke Ignore Raycast kalau kamu mau klik HP masih berfungsi
            // gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
}
