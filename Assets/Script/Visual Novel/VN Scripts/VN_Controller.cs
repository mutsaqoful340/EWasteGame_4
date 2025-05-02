using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VN_Controller : MonoBehaviour
{
    public StoryScene currentScene;
    public BottomBarController bottomBar;

    // Start is called before the first frame update
    void Start()
    {
        bottomBar.PlayScene(currentScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (bottomBar.IsCompleted())
            {
                bottomBar.PlayNextSentence();
            }
        }
    }
}
