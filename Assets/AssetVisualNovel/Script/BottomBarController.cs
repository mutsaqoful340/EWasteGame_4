using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BottomBarController : MonoBehaviour
{
    public TextMeshProUGUI barText;
    public TextMeshProUGUI personNameText;

    private int sentenceIndex = 0;
    private StoryScene currentScene;
    private State state = State.COMPLETED;
    private Coroutine typingCoroutine;

    private enum State
    {
        PLAYING, COMPLETED
    }

    public void PlayScene(StoryScene scene)
    {
        if (currentScene != scene)
        {
            currentScene = scene;
            sentenceIndex = 0;
        }

        PlayNextSentence();
    }

    public void PlayNextSentence()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (sentenceIndex >= currentScene.sentences.Count)
        {
            state = State.COMPLETED;
            return;
        }

        var sentence = currentScene.sentences[sentenceIndex];
        typingCoroutine = StartCoroutine(TypeText(sentence.text));

        personNameText.text = sentence.speaker.speakerName;
        personNameText.color = sentence.speaker.textColor;

        sentenceIndex++; // Pindah ke kalimat berikutnya setelah ditampilkan
    }

    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }

    public bool IsLastSentence()
    {
        return sentenceIndex >= currentScene.sentences.Count;
    }

    private IEnumerator TypeText(string fullText)
    {
        barText.text = "";
        state = State.PLAYING;

        for (int i = 0; i < fullText.Length; i++)
        {
            barText.text += fullText[i];
            yield return new WaitForSeconds(0.05f);
        }

        state = State.COMPLETED;
    }
}
