using UnityEngine;

// Manages audio playback for the game, providing sounds for flips, matches, mismatches, etc.
public class AudioService : MonoBehaviour
{
    [SerializeField] private AudioClip flipSound;
    [SerializeField] private AudioClip matchSound;
    [SerializeField] private AudioClip mismatchSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Plays a specific sound for card flipping
    public void PlayFlipSound() => audioSource.PlayOneShot(flipSound);

    // Plays a sound when a match is found
    public void PlayMatchSound() => audioSource.PlayOneShot(matchSound);

    // Plays a sound when cards do not match
    public void PlayMismatchSound() => audioSource.PlayOneShot(mismatchSound);
}
