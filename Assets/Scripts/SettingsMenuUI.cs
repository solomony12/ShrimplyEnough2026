using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;
    public Slider sensitivitySlider;

    public Button resumeButton;
    public Button mainMenuButton;

    private const string mainMenuSceneString = "MainMenu";
    private const string settingsSceneString = "Settings";

    public static bool SettingsIsOpen = false;

    public AudioClip testSound;

    private void Start()
    {
        // Load saved values into sliders
        musicSlider.value = GameSettings.Instance.musicVolume;
        sfxSlider.value = GameSettings.Instance.sfxVolume;
        voiceSlider.value = GameSettings.Instance.voiceVolume;
        sensitivitySlider.value = GameSettings.Instance.mouseSensitivity;

        // Add listeners
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        voiceSlider.onValueChanged.AddListener(OnVoiceChanged);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        // Check which screen
        if (SceneManager.GetActiveScene().name == mainMenuSceneString)
        {
            resumeButton.interactable = false;
        }
        else
        {
            resumeButton.interactable = true;
        }
    }

    public void OnMusicChanged(float value)
    {
        GameSettings.Instance.musicVolume = value;
        GameSettings.Instance.SaveSettings();
        AudioManager.Instance.PlayMusicOneShot(testSound);
    }

    public void OnSFXChanged(float value)
    {
        GameSettings.Instance.sfxVolume = value;
        GameSettings.Instance.SaveSettings();
        AudioManager.Instance.PlaySFX(testSound);
    }

    public void OnVoiceChanged(float value)
    {
        GameSettings.Instance.voiceVolume = value;
        GameSettings.Instance.SaveSettings();
        AudioManager.Instance.PlayVoiceOneShot(testSound);
    }

    public void OnSensitivityChanged(float value)
    {
        GameSettings.Instance.mouseSensitivity = value;
        GameSettings.Instance.SaveSettings();
    }

    public void ResumeGame()
    {
        SettingsIsOpen = false;
        if (!SceneManager.GetSceneByName(mainMenuSceneString).isLoaded)
        {
            Cursor.lockState = CursorLockMode.Locked;
            PlayerController.EnablePlayerControl();
        }
        if (SceneManager.GetSceneByName(settingsSceneString).isLoaded)
        {
            SceneManager.UnloadSceneAsync(settingsSceneString);
        }
    }

    public void MainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SettingsIsOpen = false;

        if (SceneManager.GetSceneByName(mainMenuSceneString).isLoaded)
        {
            //AudioClip mainMusic = Resources.Load<AudioClip>("Music/main");
            //AudioManager.Instance.PlayMusic(mainMusic, true);
            SceneManager.UnloadSceneAsync(settingsSceneString);
        }
        else
        {
            ScanEvidence.IsDisplayOpen = false;
            Cursor.lockState = CursorLockMode.None;
            SceneTransition.Instance.StartTransition(mainMenuSceneString);
        }
    }
}
