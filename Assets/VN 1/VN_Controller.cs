using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VN_Controller : MonoBehaviour
{
    public VN_StoryScene currentScene;
    public VN_BottomBarController bottomBar;
    public VN_BGCtrl BGController;

    void Start()
    {
        // Set background pertama
        BGController.SetImage(currentScene.background);

        // Mainkan scene pertama
        bottomBar.PlayScene(currentScene);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            if (bottomBar.IsCompleted())
            {
                if (bottomBar.IsLastSentence())
                {
                    // Cek jika ada scene berikutnya
                    if (currentScene.nextScene != null)
                    {
                        // Ganti ke scene berikutnya
                        currentScene = currentScene.nextScene;

                        // Ganti background
                        BGController.SwitchImage(currentScene.background);

                        // Mainkan scene baru dari kalimat pertama
                        bottomBar.PlayScene(currentScene);
                    }
                    else
                    {
                        // Sudah tidak ada scene, bisa kasih log atau end game
                        Debug.Log("End of visual novel");
                    }
                }
                else
                {
                    // Lanjut ke kalimat berikutnya
                    bottomBar.PlayNextSentence();
                }
            }
        }
    }
}
