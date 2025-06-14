using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VN_SpriteCtrl : MonoBehaviour
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
    [SerializeField] private float hideAnimationDelay = 0.5f;

    private VN_Speaker currentSpeaker;
    private Animator animator;
    private RectTransform rect;
    private Coroutine movementCoroutine;
    private bool isAnimating = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();

        if (characterImage == null)
        {
            characterImage = GetComponentInChildren<Image>();
        }

        // Hide character at start
        characterImage.color = new Color(1, 1, 1, 0);
    }

    public void HandleCharacter(VN_Speaker speaker)
    {
        if (isAnimating) return;

        if (speaker == null)
        {
            StartCoroutine(HideCharacterRoutine());
        }
        else if (currentSpeaker != speaker)
        {
            StartCoroutine(SwitchCharacterRoutine(speaker));
        }
    }

    public void Move(Vector2 coords, float speed)
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(MoveCoroutine(coords, speed));
    }

    private IEnumerator HideCharacterRoutine()
    {
        isAnimating = true;
        animator.SetTrigger("CharHide");
        yield return new WaitForSeconds(hideAnimationDelay);
        currentSpeaker = null;
        isAnimating = false;
    }

    private IEnumerator SwitchCharacterRoutine(VN_Speaker newSpeaker)
    {
        isAnimating = true;

        // Hide current character if exists
        if (currentSpeaker != null)
        {
            animator.SetTrigger("CharHide");
            yield return new WaitForSeconds(hideAnimationDelay);
        }

        // Show new character
        CharacterData charData = GetCharacterData(newSpeaker);
        if (charData != null)
        {
            characterImage.sprite = charData.sprite;
            rect.anchoredPosition = charData.defaultPosition;
            currentSpeaker = newSpeaker;
            animator.SetTrigger("CharShow");
        }

        isAnimating = false;
    }

    private IEnumerator MoveCoroutine(Vector2 coords, float speed)
    {
        while (rect.localPosition.x != coords.x || rect.localPosition.y != coords.y)
        {
            rect.localPosition = Vector2.MoveTowards(rect.localPosition, coords, Time.deltaTime * 1000f * speed);
            yield return null;
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
}