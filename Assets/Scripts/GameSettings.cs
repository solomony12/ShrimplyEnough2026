using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    [Header("Audio")]
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float voiceVolume = 1f;

    [Header("Controls")]
    public float mouseSensitivity = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.SetFloat("voiceVolume", voiceVolume);
        PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivity);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume", musicVolume);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", sfxVolume);
        voiceVolume = PlayerPrefs.GetFloat("voiceVolume", voiceVolume);
        mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity", mouseSensitivity);
    }
}
