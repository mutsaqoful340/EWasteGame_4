using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VN_Controller : MonoBehaviour
{
    public GameScene currentScene;
    public VN_BottomBarController bottomBar;
    public VN_BGCtrl backgroundController;
    private State state = State.IDLE;
    public GameObject sceneEndPanel;

    private enum State
    {
        IDLE, ANIMATE, CHOOSE
    }

    void Start()
    {
        if (currentScene is VN_StoryScene)
        {
            VN_StoryScene storyScene = currentScene as VN_StoryScene;
            bottomBar.PlayScene(storyScene);
            backgroundController.SetImage(storyScene.background);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (state == State.IDLE && bottomBar.IsCompleted())
            {
                if (bottomBar.IsLastSentence())
                {
                    var next = (currentScene as VN_StoryScene).nextScene;
                    if (next == null)
                    {
                        Debug.Log("Reached the end of story. Activating end panel.");
                        if (sceneEndPanel != null)
                            sceneEndPanel.SetActive(true);
                    }
                    else
                    {
                        PlayScene(next);
                    }
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
            }
        }
    }

    public void PlayScene(GameScene scene)
    {
        StartCoroutine(SwitchScene(scene));
    }

    private IEnumerator SwitchScene(GameScene scene)
    {
        state = State.ANIMATE;
        currentScene = scene;

        // Hide character when scene changes
        if (bottomBar.characterController != null)
            bottomBar.characterController.HandleCharacter(null);

        bottomBar.Hide();
        yield return new WaitForSeconds(1f);

        if (scene is VN_StoryScene)
        {
            VN_StoryScene storyScene = scene as VN_StoryScene;
            backgroundController.SwitchImage(storyScene.background);
            yield return new WaitForSeconds(1f);

            // Reset speaker tracking
            bottomBar.lastSpeaker = null;

            bottomBar.ClearText();
            bottomBar.Show();
            yield return new WaitForSeconds(1f);
            bottomBar.PlayScene(storyScene);
        }
        state = State.IDLE;
    }

}
