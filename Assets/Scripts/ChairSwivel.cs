using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChairSwivel : MonoBehaviour
{
    [Header("Swivel Settings")]
    [Tooltip("Maximum degrees applied per hit")]
    public float maxSwivelPerHit = 1.5f;

    [Tooltip("How much vertical difference is allowed before ignoring")]
    public float maxVerticalDifference = 2.0f;

    public void SwivelFromHit(Vector3 playerPosition)
    {
        Vector3 toPlayer = playerPosition - transform.position;

        // Ignore only if player is clearly on top
        if (toPlayer.y > maxVerticalDifference)
            return;

        toPlayer.y = 0f;

        if (toPlayer.sqrMagnitude < 0.0001f)
            return;

        toPlayer.Normalize();

        // Side factor: -1 (left) to 1 (right)
        float side = Vector3.Dot(transform.right, toPlayer);

        // Ensure there's always some rotation
        float swivel = Mathf.Clamp(side, -1f, 1f) * maxSwivelPerHit;

        transform.Rotate(0f, swivel, 0f, Space.World);
    }
}
