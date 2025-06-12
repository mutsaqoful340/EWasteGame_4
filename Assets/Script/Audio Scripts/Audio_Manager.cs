using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip BGM1;
    public AudioClip buttonClick;

    // Singleton instance
    public static Audio_Manager Instance { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            InitializeAudio(); // Start music if needed
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    private void Start()
    {
        // Optional: Auto-play BGM if not already playing
        if (!musicSource.isPlaying && BGM1 != null)
        {
            musicSource.clip = BGM1;
            musicSource.Play();
        }
    }

    // Initialize audio (called in Awake)
    private void InitializeAudio()
    {
        if (musicSource != null && BGM1 != null && !musicSource.isPlaying)
        {
            musicSource.clip = BGM1;
            musicSource.Play();
        }
    }

    // Original SFX method (unchanged)
    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }
}