using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private const string playSceneName = "1_IntroScene";
    [SerializeField] private const string tutorialSceneName = "TutorialVideo";
    [SerializeField] private const string settingsSceneName = "Settings";

    public Button playButton;

    private void Awake()
    {
        playButton.interactable = false;
    }

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneTransition.Instance.StartOpeningCutscene(playSceneName);
    }

    public void Tutorial()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(tutorialSceneName, LoadSceneMode.Additive);
        playButton.interactable = true;
    }

    public void Settings()
    {
        SettingsMenuUI.SettingsIsOpen = true;
        SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
    }

    public void Quit()
    {
        Debug.Log("Quit button pressed.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
