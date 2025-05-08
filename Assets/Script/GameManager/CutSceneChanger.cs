using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Collections;

public class CutSceneChanger : MonoBehaviour
{
    public PlayableDirector director; // Assign your Timeline's PlayableDirector
    public string sceneName;

    void OnEnable()
    {
        if (director != null)
        {
            director.stopped += OnTimelineStopped;
        }
    }

    void OnDisable()
    {
        if (director != null)
        {
            director.stopped -= OnTimelineStopped;
        }
    }

    void OnTimelineStopped(PlayableDirector pd)
    {
        // Only react if it's the right director
        if (pd == director)
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        Debug.Log("Starting to load scene: " + sceneName);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Prevent immediate activation

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true; // Allow scene activation
                Debug.Log("Scene loaded, activating...");
            }

            yield return null;
        }

        Debug.Log("Scene has been activated.");
    }
}
