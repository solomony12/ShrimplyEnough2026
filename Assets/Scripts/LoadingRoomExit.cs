using System.Collections;
using UnityEngine;

public class LoadingRoomExit : MonoBehaviour
{
    public string nextSectionName;
    public Transform exitAnchor; // in loading room

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(HandleExit());
    }

    private IEnumerator HandleExit()
    {
        yield return TransitionManager.Instance.LoadNextSection(nextSectionName, exitAnchor);
        yield return TransitionManager.Instance.UnloadSection("1_IntroScene");
    }
}
