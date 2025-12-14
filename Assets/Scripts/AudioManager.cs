using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    void Awake() // Initialize the audio source if not assigned
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    // Play a sound clip assigned
    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("No AudioClip assigned!");
            return;
        }

        audioSource.PlayOneShot(clip);
    }
}
