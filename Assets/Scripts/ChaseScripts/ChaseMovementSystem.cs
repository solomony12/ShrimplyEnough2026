using UnityEngine;

public class ChaseMovementSystem : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 7f;
    [SerializeField] private float runJiggleAmount = 0.01f;
    [SerializeField] private float runJiggleFrequency = 20f;

    [Header("Slowdown Settings")]
    [SerializeField] private float slowDownMultiplier = 0.4f;
    [SerializeField] private float slowDownDuration = 1.0f;

    private float currentSpeed;
    private float slowDownTimer = 0f;

    private Vector3 startPosition;

    private void Awake()
    {
        currentSpeed = baseSpeed;
        startPosition = transform.position;
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    private void Update()
    {
        HandleSlowDownTimer();
        MoveForwardWithJiggle();
    }

    private void HandleSlowDownTimer()
    {
        if (slowDownTimer > 0)
        {
            slowDownTimer -= Time.deltaTime;

            // When timer ends, restore speed
            if (slowDownTimer <= 0)
            {
                currentSpeed = baseSpeed;
            }
        }
    }

    private void MoveForwardWithJiggle()
    {
        Transform cam = Camera.main.transform;

        float speed = currentSpeed;

        // Move forward based on camera direction
        Vector3 forwardMove = cam.forward * speed * Time.deltaTime;
        cam.position += forwardMove;

        // Jiggle effect based on camera right
        float jiggle = Mathf.Sin(Time.time * runJiggleFrequency) * runJiggleAmount;
        cam.position += cam.right * jiggle;
    }


    // Call this from QuicktimeEventTrigger when player fails
    public void TriggerSlowDown()
    {
        slowDownTimer = slowDownDuration;
        currentSpeed = baseSpeed * slowDownMultiplier;
    }
}
