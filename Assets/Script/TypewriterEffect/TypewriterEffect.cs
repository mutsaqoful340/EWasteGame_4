using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.05f;
    public TextMeshProUGUI textComponent;
    [TextArea] public string fullText;
    public AudioSource typingSound;
    public float typingVolume = 1.0f;  // Volume suara ketikan

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

            // Menyesuaikan volume suara ketikan
            typingSound.volume = typingVolume;

            // Mainkan suara ketikan
            if (c != ' ' && c != '\n' && typingSound != null)
            {
                typingSound.Play();
            }

            yield return new WaitForSeconds(delay);
        }
    }
}
