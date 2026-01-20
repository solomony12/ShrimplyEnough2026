using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{

    [Header("Scene Info")]
    public string nextSectionName;

    [Header("Anchors")]
    public Transform exitAnchor;

    private bool isLoaded = false;

    private void OnTriggerEnter(Collider other)
    {

        if (!isLoaded)
        {
            isLoaded = true;
            StartCoroutine(TransitionManager.Instance.GoToLoadingRoom(
                SceneManager.GetActiveScene().name,
                exitAnchor
            ));
        }
    }
}
