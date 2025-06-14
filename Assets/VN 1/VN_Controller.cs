using System.Collections;
using UnityEngine;

public class VN_Controller : MonoBehaviour
{
    // Existing fields
    public VN_StoryScene currentScene;
    public VN_BottomBarController bottomBar;
    public VN_SpriteSw BGController;

    // New field
    public GameObject endSceneObject; // Assign in Inspector

    private State state = State.IDLE;

    private enum State
    {
        IDLE, ANIMATE
    }

    void Start()
    {
        if (endSceneObject != null)
            endSceneObject.SetActive(false); // Ensure hidden at start

        bottomBar.PlayScene(currentScene);
        BGController.SetImage(currentScene.background);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            if (state == State.IDLE && bottomBar.IsCompleted())
            {
                if (bottomBar.IsLastSentence())
                {
                    if (currentScene.nextScene != null)
                    {
                        PlayScene(currentScene.nextScene);
                    }
                    else
                    {
                        // No more scenes - activate the object
                        StartCoroutine(HandleEndScene());
                    }
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
            }
        }
    }

    private IEnumerator HandleEndScene()
    {
        // Optional: Add delay or fade-out animation first
        yield return new WaitForSeconds(0f);

        if (endSceneObject != null)
        {
            endSceneObject.SetActive(true);
            Debug.Log("Visual novel completed - end object activated");
        }

        // Optional: Disable the VN system
        //this.enabled = false;
    }

    private void PlayScene(VN_StoryScene scene)
    {
        StartCoroutine(SwitchScene(scene));
    }

    private IEnumerator SwitchScene(VN_StoryScene scene)
    {
        state = State.ANIMATE;
        currentScene = scene;
        bottomBar.Hide();
        yield return new WaitForSeconds(1f);
        BGController.SwitchImage(scene.background);
        yield return new WaitForSeconds(1f);
        bottomBar.ClearText();
        bottomBar.Show();
        yield return new WaitForSeconds(1f);
        bottomBar.PlayScene(scene);
        state = State.IDLE;
    }
}