using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private static float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;
    [SerializeField] private float crawlMultiplier = 0.75f;

    [Header("Look Sensitivity")]
    [SerializeField] private float mouseSensitivity => GameSettings.Instance.mouseSensitivity;
    [SerializeField] private float upDownRange = 80.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 3.0f;
    [SerializeField] private float gravity = 9.81f;

    [Header("Crawl Parameters")]
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float crawlingHeight = 1.0f;
    [SerializeField] private float crawlTransitionTime = 0.25f;

    [Header("Crouch Collision")]
    [SerializeField] private LayerMask obstructionMask;

    private bool lastCrawlState;
    private float crawlProgress = 1f;
    private float startHeight;
    private float targetHeight;
    private Vector3 startCenter;
    private Vector3 targetCenter;
    private float startCamHeight;
    private float targetCamHeight;

    [Header("Camera Parameters")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float cameraStandingHeight = 1.6f;
    [SerializeField] private float cameraCrawlingHeight = 0.8f;

    [Header("Starting Information")]
    private Vector3 gameStartPlayerPos = new Vector3(13.2f, 1.08f, 1.4f);
    private Vector3 gameStartCameraRot = new Vector3(351.826538f, 0f, 0f);

    [Header("Slippery Settings")]
    [SerializeField] private float slipperyFriction = 0.0001f;
    private bool isOnSlipperySurface = false;
    private Vector3 slideVelocity = Vector3.zero;

    private CharacterController characterController;
    private Camera mainCamera;
    private PlayerInputHandler inputHandler;
    private GameObject player;
    private Vector3 currentMovement;
    private float verticalRotation;
    private static bool canControlCharacter;

    private static bool isConstantlyRunning = false;

    private Rigidbody rb;
    private bool usingRigidbody = false;
    private CapsuleCollider capsuleCollider;


    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        player = GameObject.FindWithTag("Player");
        inputHandler = PlayerInputHandler.Instance;
        cameraPivot = mainCamera.transform;
        canControlCharacter = true;
        ResetRunningConstantly();
    }

    public static void EnablePlayerControl()
    {
        canControlCharacter = true;
    }

    public static void DisablePlayerControl()
    {
        canControlCharacter = false;
    }

    public static void RunningConstantly()
    {
        walkSpeed = 8.0f;
        isConstantlyRunning = true;
    }

    public static void ResetRunningConstantly()
    {
        walkSpeed = 3.0f;
        isConstantlyRunning = false;
    }

    public float GetCameraStandingHeight()
    {
        return cameraStandingHeight;
    }

    public bool CanPlayerControl() { return canControlCharacter; }

    private void Update()
    {
        try
        {
            if (canControlCharacter)
            {
                HandleMovement();
                HandleRotation();
            }
        }
        catch (Exception e)
        {
        }
    }

    void HandleMovement()
    {
        float multiplier = 1f;

        if (inputHandler.CrawlTriggered)
        {
            multiplier = crawlMultiplier;
        }
        else if (!isConstantlyRunning && inputHandler.SprintValue > 0)
        {
            multiplier = sprintMultiplier;
        }

        float speed = walkSpeed * multiplier;

        Vector3 inputDirections = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirections);
        worldDirection.Normalize();

        Vector3 desiredMove = worldDirection * speed;

        if (isOnSlipperySurface)
        {
            // Slide while still allowing small control
            slideVelocity = Vector3.Lerp(slideVelocity, desiredMove, slipperyFriction);
            currentMovement.x = slideVelocity.x;
            currentMovement.z = slideVelocity.z;
        }
        else
        {
            slideVelocity = desiredMove;
            currentMovement.x = desiredMove.x;
            currentMovement.z = desiredMove.z;
        }

        try
        {
            HandleJumping();
            HandleCrawling();
            characterController.Move(currentMovement * Time.deltaTime);
        }
        catch (Exception e)
        {

        }
    }

    void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (inputHandler.JumpTriggered)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
    }

    void HandleCrawling()
    {
        if (characterController.isGrounded)
        {
            bool crawling = inputHandler.CrawlTriggered;

            // IF on slippery surface and already crouched, force crawl
            if (isOnSlipperySurface && characterController.height < standingHeight)
            {
                crawling = true;
            }

            // If player wants to stand but something is blocking, force crouch
            if (!crawling && !CanStandUp())
            {
                crawling = true;
            }

            // Only reset transition when state actually changes
            if (crawling != lastCrawlState)
            {
                lastCrawlState = crawling;
                crawlProgress = 0f;

                startHeight = characterController.height;
                targetHeight = crawling ? crawlingHeight : standingHeight;

                startCenter = characterController.center;
                targetCenter = new Vector3(0, targetHeight / 2f, 0);

                startCamHeight = cameraPivot.localPosition.y;
                targetCamHeight = crawling ? cameraCrawlingHeight : cameraStandingHeight;
            }

            // Continue transition even if player spams
            if (crawlProgress < 1f)
            {
                crawlProgress += Time.deltaTime / crawlTransitionTime;
                crawlProgress = Mathf.Clamp01(crawlProgress);

                characterController.height = Mathf.Lerp(startHeight, targetHeight, crawlProgress);
                characterController.center = Vector3.Lerp(startCenter, targetCenter, crawlProgress);

                Vector3 camPos = cameraPivot.localPosition;
                camPos.y = Mathf.Lerp(startCamHeight, targetCamHeight, crawlProgress);
                cameraPivot.localPosition = camPos;
            }
        }
    }

    bool CanStandUp()
    {
        float radius = characterController.radius;
        Vector3 bottom = transform.position + Vector3.up * radius;
        Vector3 top = transform.position + Vector3.up * (standingHeight - radius);

        return !Physics.CheckCapsule(
            bottom,
            top,
            radius,
            obstructionMask,
            QueryTriggerInteraction.Ignore
        );
    }


    void HandleRotation()
    {
        float mouseXRotation = inputHandler.LookInput.x * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);

        verticalRotation -= inputHandler.LookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    public void StartingPositionSet(float duration = 1f)
    {
        StopAllCoroutines();
        StartCoroutine(LerpToStart(duration));
    }

    private IEnumerator LerpToStart(float duration)
    {
        Vector3 startPos = player.transform.position;
        Quaternion startRot = mainCamera.transform.localRotation;

        Vector3 targetPos = gameStartPlayerPos;
        Quaternion targetRot = Quaternion.Euler(gameStartCameraRot);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            player.transform.position = Vector3.Lerp(startPos, targetPos, t);
            mainCamera.transform.localRotation = Quaternion.Lerp(startRot, targetRot, t);

            yield return null;
        }

        // Ensure exact final values
        player.transform.position = targetPos;
        mainCamera.transform.localRotation = targetRot;
    }

    /// SLIPPERY STUFF

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Slippery"))
        {
            Debug.Log("slippery");
            isOnSlipperySurface = true;
        }
        else
        {
            isOnSlipperySurface = false;
        }
    }
}
