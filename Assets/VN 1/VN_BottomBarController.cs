using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine.UIElements;
using UnityEngine.UI;


public class VN_BottomBarController : MonoBehaviour
{
    [SerializeField] private Button nextButton;

    [Header("Character Settings")]
    public VN_SpriteCtrl characterController;
    public VN_Speaker lastSpeaker;

    public TextMeshProUGUI barText;
    public TextMeshProUGUI personNameText;

    private int sentenceIndex = -1;
    private VN_StoryScene currentScene;
    private State state = State.COMPLETED;
    private Animator animator;
    private bool isHidden = false;

    private enum State
    {
        PLAYING, COMPLETED
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Hide()
    {
        if (!isHidden)
        {
            animator.SetTrigger("BBar_Hide");
            isHidden = true;
        }
    }

    public void Show()
    {
        animator.SetTrigger("BBar_Show");
        isHidden = false;
    }

    public void ClearText()
    {
        barText.text = "";
    }

    public void PlayScene(VN_StoryScene scene)
    {
        currentScene = scene;
        sentenceIndex = -1;
        PlayNextSentence();
    }

    public void PlayNextSentence()
    {
        var sentence = currentScene.sentences[++sentenceIndex];

        // Handle character changes
        if (characterController != null)
        {
            characterController.HandleCharacter(sentence.speaker);
        }

        lastSpeaker = sentence.speaker;
        StartCoroutine(TypeText(sentence.text));
        UpdateSpeakerUI(sentence.speaker);
    }

    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }

    public bool IsLastSentence()
    {
        return sentenceIndex + 1 == currentScene.sentences.Count;
    }

    private IEnumerator TypeText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            state = State.COMPLETED;
            yield break;
        }

        if (nextButton != null)
            nextButton.interactable = false;

        barText.text = "";
        state = State.PLAYING;

        int wordIndex = 0;

        while (wordIndex < text.Length)
        {
            barText.text += text[wordIndex];
            yield return new WaitForSeconds(0.025f);
            wordIndex++;
        }

        state = State.COMPLETED;

        if (nextButton != null)
            nextButton.interactable = true;
    }



    private void UpdateSpeakerUI(VN_Speaker speaker)
    {
        personNameText.text = speaker.SpeakerName;
        personNameText.color = speaker.textColor;
    }
}