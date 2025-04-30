using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VNLoader : MonoBehaviour
{
    public void LoadVNScene()
    {
        StartCoroutine(LoadVNDelayed());
    }

    private IEnumerator LoadVNDelayed()
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("VN", LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadOp.isDone);

        // Cari kamera di scene VN
        GameObject camObj = GameObject.Find("VN_Camera");

        if (camObj != null)
        {
            Debug.Log("VN Camera found: " + camObj.name);
            Debug.Log("VN Camera Active: " + camObj.activeInHierarchy);

            Camera cam = camObj.GetComponent<Camera>();
            if (cam != null && cam.targetTexture != null)
            {
                Debug.Log("Camera target texture: " + cam.targetTexture.name);
            }
            else
            {
                Debug.LogWarning("VN Camera tidak punya targetTexture atau komponen Camera.");
            }
        }
        else
        {
            Debug.LogWarning("VN Camera TIDAK ditemukan di scene VN.");
        }
    }
}
