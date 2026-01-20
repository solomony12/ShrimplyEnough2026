using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorEntryTrigger : MonoBehaviour
{
    private Door door;

    private bool isLoaded = false;

    public string previousSectionName;

    private void Awake()
    {
        door = GetComponentInParent<Door>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (door == null) return;
        if (!door.IsOneWay()) return;
        if (!other.CompareTag("Player")) return;

        door.CloseAndLock();

        if (!isLoaded)
        {
            isLoaded = true;
            TransitionManager.Instance.UnloadSection(previousSectionName);
        }
    }
}
