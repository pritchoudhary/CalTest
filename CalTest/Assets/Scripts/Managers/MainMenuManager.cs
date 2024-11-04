using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages the main menu actions, including navigation, volume adjustment, and mute toggle.
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; // Slider for volume control
    [SerializeField] private Toggle muteToggle;   // Toggle for muting/unmuting audio

    private void Start()
    {
        // Initialize volume and mute controls with saved settings
        volumeSlider.value = AudioManager.Instance.GetVolume();
        muteToggle.isOn = AudioManager.Instance.IsMuted();

        // Attach listeners to handle changes in volume and mute controls
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggled);
    }
    // Loads the level by its build index as specified in the Build Settings
    public void LoadLevelByIndex(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogError("Invalid level index. Make sure the level is added to Build Settings.");
        }
    }

    // Quits the application (only works in a built application)
    public void QuitGame()
    {
        Application.Quit();
    }

    // Called when volume slider is adjusted, updates volume in AudioManager
    private void OnVolumeChanged(float volume)
    {
        AudioManager.Instance.SetVolume(volume);
    }

    // Called when mute toggle is changed, updates mute state in AudioManager
    private void OnMuteToggled(bool isMuted)
    {
        AudioManager.Instance.SetMute(isMuted);
    }
}
