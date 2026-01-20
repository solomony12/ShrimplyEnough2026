using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    private static DoNotDestroy current;

    private void Awake()
    {
        // If an old instance exists, destroy it
        if (current != null && current != this)
        {
            Destroy(current.gameObject);
        }

        // Set this as the current instance
        current = this;

        // Persist across scenes
        DontDestroyOnLoad(gameObject);
    }
}