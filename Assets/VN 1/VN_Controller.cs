using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VN_Controller : MonoBehaviour
{
    public VN_StoryScene currentScene;
    public VN_BottomBarController bottomBar;

    void Start()
    {
        bottomBar.PlayScene(currentScene);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            if (bottomBar.IsCompleted())
            {
                bottomBar.PlayNextSentence();
            }
        }
    } 
}
