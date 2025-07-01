using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk ganti scene

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.05f;
    public TextMeshProUGUI textComponent;
    [TextArea] public string fullText;
    public AudioSource typingSound;
    public float typingVolume = 1.0f;
    public string nextSceneName = "SceneBerikutnya"; // Ganti dengan nama scene kamu
    public float delayBeforeNextScene = 1.5f; // Waktu jeda sebelum pindah scene

    private void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        textComponent.text = "";

        foreach (char c in fullText)
        {
            textComponent.text += c;

            if (typingSound != null && c != ' ' && c != '\n')
            {
                typingSound.volume = typingVolume;

                if (typingSound.isPlaying)
                    typingSound.Stop();

                typingSound.Play();
            }

            yield return new WaitForSeconds(delay);
        }

        // Pastikan suara berhenti
        if (typingSound != null && typingSound.isPlaying)
        {
            typingSound.Stop();
        }

        // Tunggu sebentar sebelum pindah scene
        yield return new WaitForSeconds(delayBeforeNextScene);

        // Ganti ke scene berikutnya
        SceneManager.LoadScene(nextSceneName);
    }
}
