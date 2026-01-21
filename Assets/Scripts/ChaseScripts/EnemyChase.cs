using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyChase : MonoBehaviour
{
    public enum TurnDirection { Left, Right }

    [Header("Movement Settings")]
    private float moveSpeed = 5.45f;
    private float rayDistance = 2.35f;

    [Header("Turn Pattern")]
    public TurnDirection[] turnPattern;

    [Header("Wall Detection")]
    public LayerMask wallLayer;

    private int currentTurnIndex = 0;
    private bool isStopped = false;

    void Update()
    {
        if (isStopped) return;

        MoveForward();
        CheckForWall();
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void CheckForWall()
    {
        if (Physics.Raycast(transform.position, transform.forward, rayDistance, wallLayer))
        {
            Turn();
        }
    }

    void Turn()
    {
        if (turnPattern.Length == 0)
            return;

        TurnDirection direction = turnPattern[currentTurnIndex];

        if (direction == TurnDirection.Left)
            transform.Rotate(0, -90, 0);
        else
            transform.Rotate(0, 90, 0);

        currentTurnIndex++;

        if (currentTurnIndex >= turnPattern.Length)
            currentTurnIndex = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isStopped = true;
            TriggerJumpscare();
        }
    }

    private void TriggerJumpscare()
    {
        PlayerController.DisablePlayerControl();

        // Lerp to animation

        // Play jumpscare animation

        // Load game over scene
        SceneManager.LoadScene("TimedGameOverScene");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayDistance);
    }
}
