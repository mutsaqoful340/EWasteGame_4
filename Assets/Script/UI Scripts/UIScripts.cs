using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; //To access UI elements in Unity, add this.

public class UIScripts : MonoBehaviour
{
    // To declare the animator or smth idk
    private Animator animator;

    // 3D Canvas Navigation using animation Parameters' name
    public void PlayCameraAnimation(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning("No Animator found on the Camera!");
        }
    }

    public void Btn_PlayGame()
    {
        //Load scene by <Build Index> -> SceneManager.LoadSccene(X);
        //Load scene by <Scene Name> -> SceneManager.LoadScene("NamaScene");
        //Load scene by <Order in Build Index> -> SceneManager.Loadscene(SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene("ProfileSaves");
    }

    public void Btn_Options()
    {
        SceneManager.LoadScene("MainOption");
    }

    public void Btn_BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Btn_ToVisualNovel()
    {
        SceneManager.LoadScene("VisualNovel");

    }
}