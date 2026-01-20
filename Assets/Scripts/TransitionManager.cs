using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator GoToLoadingRoom(string fromSection, Transform fromDoorExitAnchor)
    {
        // 1) Load loading room
        yield return SceneManager.LoadSceneAsync("LoadingRoom", LoadSceneMode.Additive);

        // 2) Align loading room entry
        AlignScene("LoadingRoom", fromDoorExitAnchor, "EntryAnchor");

        // 3) Set loading room as active scene
        Scene loadedScene = SceneManager.GetSceneByName("LoadingRoom");
        SceneManager.SetActiveScene(loadedScene);

        yield return null;
    }

    public IEnumerator LoadNextSection(string nextSection, Transform loadingRoomExitAnchor)
    {
        // 1) Load next section
        yield return SceneManager.LoadSceneAsync(nextSection, LoadSceneMode.Additive);

        // 2) Align next section entry anchor with loading room exit
        AlignScene(nextSection, loadingRoomExitAnchor, "EntryAnchor");

        // 3) Set new section as active scene
        Scene loadedScene = SceneManager.GetSceneByName(nextSection);
        SceneManager.SetActiveScene(loadedScene);

        yield return null;
    }

    public IEnumerator UnloadSection(string sectionName)
    {
        yield return SceneManager.UnloadSceneAsync(sectionName);
    }

    private void AlignScene(string sceneName, Transform anchorToMatch, string anchorNameInScene)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        GameObject[] roots = scene.GetRootGameObjects();

        GameObject container = null;
        foreach (GameObject obj in roots)
        {
            if (obj.name == "SceneContainer")
            {
                container = obj;
                break;
            }
        }

        if (container == null)
        {
            Debug.LogError("SceneContainer not found in " + sceneName);
            return;
        }

        Transform anchor = container.transform.Find(anchorNameInScene);
        if (anchor == null)
        {
            Debug.LogError("Anchor not found: " + anchorNameInScene);
            return;
        }

        // ----- POSITION -----
        Vector3 positionDelta = anchorToMatch.position - anchor.position;
        container.transform.position += positionDelta;

        // ----- ROTATION -----
        Quaternion deltaRotation = Quaternion.FromToRotation(anchor.forward, anchorToMatch.forward);
        container.transform.rotation = deltaRotation * container.transform.rotation;
    }

}
