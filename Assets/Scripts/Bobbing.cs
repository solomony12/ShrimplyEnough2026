using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [Header("Bob Settings")]
    [SerializeField] private float bobHeight = 0.3f;
    [SerializeField] private float bobSpeed = 1.0f;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Calculate vertical offset using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        // Apply new position
        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );
    }
}
