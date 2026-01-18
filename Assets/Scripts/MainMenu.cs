using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string playSceneName = "SampleScene";
    [SerializeField] private string creditsSceneName = "CreditsScene";
    [SerializeField] private string settingsSceneName = "Settings";

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
