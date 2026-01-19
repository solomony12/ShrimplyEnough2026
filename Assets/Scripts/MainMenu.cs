using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private const string playSceneName = "1_IntroScene";
    [SerializeField] private const string creditsSceneName = "CreditsScene";
    [SerializeField] private const string settingsSceneName = "Settings";

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneTransition.Instance.StartTransition(playSceneName);
    }

    public void Credits()
    {
        SceneManager.LoadScene(creditsSceneName);
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
