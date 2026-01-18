using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    [Header("Audio")]
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float voiceVolume = 1f;

    [Header("Controls")]
    public float mouseSensitivity = 2f;

    private PlayerInputHandler inputHandler;

    private float inputDelay = 0.15f;
    private float inputTimer = 0f;

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

    private void Update()
    {
        inputTimer -= Time.deltaTime;

        if (inputTimer > 0) return;

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (inputHandler == null)
                inputHandler = PlayerInputHandler.Instance;

            if (inputHandler.EscapeTriggered && !SceneTransition.IsTransitioning)
            {
                PauseResume();
                inputTimer = inputDelay;
            }
        }
    }

    private void PauseResume()
    {
        string settingsSceneString = "Settings";

        if (SceneManager.GetSceneByName(settingsSceneString).isLoaded)
        {
            if (!ScanEvidence.IsDisplayOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                PlayerController.EnablePlayerControl();
            }
            SceneManager.UnloadSceneAsync(settingsSceneString);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerController.DisablePlayerControl();
            SceneManager.LoadScene(settingsSceneString, LoadSceneMode.Additive);
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
