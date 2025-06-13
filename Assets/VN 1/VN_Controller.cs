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
        bottomBar.PlayScene(currentScene);
        BGController.SetImage(currentScene.background);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            if (bottomBar.IsCompleted())
            {
                if (bottomBar.IsLastSentence())
                {
                    currentScene = currentScene.nextScene;
                    bottomBar.PlayScene(currentScene);
                    BGController.SwitchImage(currentScene.background);
                }
                bottomBar.PlayNextSentence();
            }
        }
    } 
}
