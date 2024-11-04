using UnityEngine;

// Manages global audio settings, including volume and mute state, with persistent settings.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance for global access
    private AudioSource audioSource;     // Audio source for managing background music and sounds

    private const string VolumePrefKey = "AudioVolume"; // Key for saving volume in PlayerPrefs
    private const string MutePrefKey = "AudioMute";     // Key for saving mute state in PlayerPrefs

    private void Awake()
    {
        // Ensure only one instance of AudioManager exists (Singleton pattern)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            audioSource = GetComponent<AudioSource>();
            LoadSettings(); // Load saved audio settings
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Sets the volume and saves the setting
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(VolumePrefKey, volume); // Save volume setting
    }

    // Mutes/unmutes audio and saves the mute state
    public void SetMute(bool isMuted)
    {
        audioSource.mute = isMuted;
        PlayerPrefs.SetInt(MutePrefKey, isMuted ? 1 : 0); // Save mute state
    }

    // Gets the current volume level
    public float GetVolume()
    {
        return audioSource.volume;
    }

    // Returns true if audio is muted
    public bool IsMuted()
    {
        return audioSource.mute;
    }

    // Loads saved volume and mute settings from PlayerPrefs
    private void LoadSettings()
    {
        audioSource.volume = PlayerPrefs.GetFloat(VolumePrefKey, 1f); // Default to max volume
        audioSource.mute = PlayerPrefs.GetInt(MutePrefKey, 0) == 1;   // Default is not muted
    }
}
