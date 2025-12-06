using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusicClip;
    [SerializeField] private float volume = 0.5f;
    [SerializeField] private bool loop = true;
    
    private AudioSource audioSource;

    private void Awake()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configure AudioSource
        audioSource.clip = backgroundMusicClip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.playOnAwake = true;
    }

    private void Start()
    {
        // Play the background music
        if (backgroundMusicClip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("BackgroundMusic: No audio clip assigned! Please assign a background music clip in the Inspector.");
        }
    }
}

