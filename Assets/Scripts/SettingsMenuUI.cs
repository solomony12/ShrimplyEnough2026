using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;
    public Slider sensitivitySlider;

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
    }

    public void OnMusicChanged(float value)
    {
        GameSettings.Instance.musicVolume = value;
        GameSettings.Instance.SaveSettings();
    }

    public void OnSFXChanged(float value)
    {
        GameSettings.Instance.sfxVolume = value;
        GameSettings.Instance.SaveSettings();
    }

    public void OnVoiceChanged(float value)
    {
        GameSettings.Instance.voiceVolume = value;
        GameSettings.Instance.SaveSettings();
    }

    public void OnSensitivityChanged(float value)
    {
        GameSettings.Instance.mouseSensitivity = value;
        GameSettings.Instance.SaveSettings();
    }
}
