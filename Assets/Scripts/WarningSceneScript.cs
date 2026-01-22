using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningSceneScript : MonoBehaviour
{
    public static bool isWarningScreenUp;
    public static string previousSceneName;

    private static readonly string[] sceneNames =
    {
        "1_IntroScene",
        "2_Warehouse_Scene",
        //"3_FactoryFloor",
        "4_Office",
        //"5_ShrimpTesting",
        "6_FinalArea",
        "7_EndingLevel"
    };

    private void Awake()
    {
        if (string.IsNullOrEmpty(previousSceneName))
        {
            previousSceneName = SceneManager.GetActiveScene().name;
        }
    }

    public void Yes()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isWarningScreenUp = false;

        LoadNextScene();
    }

    public void No()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isWarningScreenUp = false;

        PlayerController.EnablePlayerControl();
        SceneManager.UnloadSceneAsync("WarningScene");
    }
    public void LoadNextScene()
    {
        string current = previousSceneName;
        Debug.Log("WarningSceneScript: LoadNextScene() called.");
        Debug.Log("WarningSceneScript: previousSceneName = " + current);

        int index = System.Array.IndexOf(sceneNames, current);
        Debug.Log("WarningSceneScript: current scene index = " + index);

        if (index < 0)
        {
            Debug.LogWarning("WarningSceneScript: Current scene not found in list: " + current);
            return;
        }

        int nextIndex = index + 1;
        Debug.Log("WarningSceneScript: next scene index = " + nextIndex);

        if (nextIndex >= sceneNames.Length)
        {
            Debug.Log("WarningSceneScript: No next scene to load. You're at the final scene.");
            return;
        }

        string nextScene = sceneNames[nextIndex];
        Debug.Log("WarningSceneScript: Loading next scene: " + nextScene);

        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
