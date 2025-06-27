using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VN_Controller : MonoBehaviour
{
    public GameScene currentScene;
    public VN_BottomBarController bottomBar;
    public Button nextButton;
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
            nextButton.onClick.AddListener(OnNextButtonClicked);

        }
    }

    private void OnNextButtonClicked()
    {
        if (bottomBar.IsCompleted())
        {
            if (!bottomBar.IsLastSentence())
            {
                bottomBar.PlayNextSentence();
            }
            else
            {
                // Optionally do something when the scene ends
                Debug.Log("Scene ended");
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
