using UnityEngine;

public class DoorEntryTrigger : MonoBehaviour
{
    private Door door;

    private void Awake()
    {
        door = GetComponentInParent<Door>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (door == null) return;

        if (!door.IsOneWay()) return;

        if (other.CompareTag("Player"))
        {
            door.CloseAndLock();
        }
    }
}
