using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public StoryScene currentScene;
    public BottomBarController bottomBar;
    public BackgroundController backgroundController;

    
    void Start()
    {
        bottomBar.PlayScene(currentScene);
        backgroundController.SetImage(currentScene.background);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Debug.Log("Tombol ditekan: Space atau Mouse Click");

            if (bottomBar.IsCompleted())
            {
                Debug.Log("Scene sudah selesai.");

                if (bottomBar.IsLastSentence())
                {
                    Debug.Log("Ini adalah kalimat terakhir.");
                    currentScene = currentScene.nextScene;
                    bottomBar.PlayScene(currentScene);
                    backgroundController.SwitchImage(currentScene.background);
                }

                bottomBar.PlayNextSentence();
            }
        }
    }

}
