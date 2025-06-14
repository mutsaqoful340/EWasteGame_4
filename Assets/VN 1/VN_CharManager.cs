using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VN_CharManager : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public VN_Speaker speaker;
        public Sprite sprite;
        public Vector2 defaultPosition = Vector2.zero;
    }

    [Header("Character Settings")]
    [SerializeField] private List<CharacterData> characterDatabase = new List<CharacterData>();
    [SerializeField] private Image characterImage;
    [SerializeField] private float switchSpeed = 1f;

    [Header("Animation Parameters")]
    [SerializeField] private string showTrigger = "Show";
    [SerializeField] private string hideTrigger = "Hide";
    [SerializeField] private string switchTrigger = "Switch";

    private Animator animator;
    private VN_Speaker currentSpeaker;
    private VN_BGCtrl backgroundController;
    private Coroutine movementCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        backgroundController = FindObjectOfType<VN_BGCtrl>();

        if (characterImage == null)
        {
            characterImage = GetComponentInChildren<Image>();
        }

        // Hide character at start
        characterImage.color = new Color(1, 1, 1, 0);
    }

    private void OnEnable()
    {
        if (backgroundController != null)
        {
            backgroundController.isSwitched = false; // Reset background state tracking
        }
    }

    public void HandleCharacterChange(VN_Speaker newSpeaker)
    {
        if (newSpeaker == null)
        {
            // Hide character if no speaker
            HideCharacter();
            return;
        }

        // Check if speaker actually changed
        if (currentSpeaker != newSpeaker)
        {
            StartCoroutine(ChangeCharacterRoutine(newSpeaker));
        }
    }

    private IEnumerator ChangeCharacterRoutine(VN_Speaker newSpeaker)
    {
        // If we have a current character, hide it first
        if (currentSpeaker != null)
        {
            animator.SetTrigger(hideTrigger);
            yield return new WaitForSeconds(GetAnimationLength(hideTrigger));
        }

        // Set new character
        currentSpeaker = newSpeaker;
        CharacterData charData = GetCharacterData(newSpeaker);

        if (charData != null)
        {
            characterImage.sprite = charData.sprite;
            characterImage.rectTransform.anchoredPosition = charData.defaultPosition;

            // Show new character
            animator.SetTrigger(showTrigger);
            yield return new WaitForSeconds(GetAnimationLength(showTrigger));
        }
    }

    public void MoveCharacter(Vector2 newPosition, float speed)
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(MoveCharacterRoutine(newPosition, speed));
    }

    private IEnumerator MoveCharacterRoutine(Vector2 newPosition, float speed)
    {
        RectTransform rect = characterImage.rectTransform;
        Vector2 startPos = rect.anchoredPosition;
        float journeyLength = Vector2.Distance(startPos, newPosition);
        float startTime = Time.time;

        while (rect.anchoredPosition != newPosition)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / journeyLength;
            rect.anchoredPosition = Vector2.Lerp(startPos, newPosition, fractionOfJourney);
            yield return null;
        }
    }

    public void HideCharacter()
    {
        if (currentSpeaker != null)
        {
            animator.SetTrigger(hideTrigger);
            currentSpeaker = null;
        }
    }

    private CharacterData GetCharacterData(VN_Speaker speaker)
    {
        foreach (CharacterData data in characterDatabase)
        {
            if (data.speaker == speaker)
            {
                return data;
            }
        }
        return null;
    }

    private float GetAnimationLength(string triggerName)
    {
        if (animator != null)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            for (int i = 0; i < ac.animationClips.Length; i++)
            {
                if (ac.animationClips[i].name.ToLower().Contains(triggerName.ToLower()))
                {
                    return ac.animationClips[i].length;
                }
            }
        }
        return 0.5f; // Default fallback duration
    }

    // Call this when background changes
    public void OnBackgroundChanged()
    {
        if (currentSpeaker != null)
        {
            // Play switch animation when background changes
            animator.SetTrigger(switchTrigger);
        }
    }
}