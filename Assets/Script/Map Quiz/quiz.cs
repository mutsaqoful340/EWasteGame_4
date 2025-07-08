using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class quiz : MonoBehaviour
{
    public Animator feedAnim; // Reference to Animator
    //public Animator incorrectAnim; // Reference to Animator for incorrect answers

    public void question(bool answer)
    {
        int score;
        if (answer)
        {
            score = PlayerPrefs.GetInt("score") + 10;
            if (feedAnim != null)
                feedAnim.Play("AswCorrect"); // Trigger correct animation
        }
        else
        {
            score = PlayerPrefs.GetInt("score") - 10;
            if (score < 0)
            {
                score = 0;
            }
            if (feedAnim != null)
                feedAnim.Play("AswIncorrect"); // Trigger incorrect animation
        }
        PlayerPrefs.SetInt("score", score);
    }

    public void NextQuestion()
    {
        transform.parent.GetChild(gameObject.transform.GetSiblingIndex() + 1).gameObject.SetActive(true);
    }
}
