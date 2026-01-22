using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public static ChangeLevel Instance;

    public int levelIndex;

    public void MoveToNextLevel()
    {
        string sceneName = "";
        switch (levelIndex)
        {
            case 2:
                sceneName = "2_Warehouse_Scene";
                break;
            case 3:
                sceneName = "3_FactoryFloor";
                break;
            case 4:
                sceneName = "4_Office";
                break;
            case 5:
                sceneName = "5_ShrimpTesting";
                break;
            case 6:
                sceneName = "6_FinalArea";
                break;
            default:
                Debug.Log("BROKEN IN ChangeLevel.cs");
                break;
        }
        SceneManager.LoadScene(sceneName);
    }

    public static void MoveToNextLevelIndex(int index)
    {
        string sceneName = "";
        switch (index)
        {
            case 2:
                sceneName = "2_Warehouse_Scene";
                break;
            case 3:
                sceneName = "3_FactoryFloor";
                break;
            case 4:
                sceneName = "4_Office";
                break;
            case 5:
                sceneName = "5_ShrimpTesting";
                break;
            case 6:
                sceneName = "6_FinalArea";
                break;
            default:
                Debug.Log("BROKEN IN ChangeLevel.cs");
                break;
        }
        SceneManager.LoadScene(sceneName);
    }
}
