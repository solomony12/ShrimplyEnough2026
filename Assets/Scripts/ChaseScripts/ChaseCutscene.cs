using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class ChaseCutscene : MonoBehaviour
{
    public static bool isChasePlaying = false;

    private bool triggered = false;

    private Camera mainCamera;
    private GameObject player;
    public GameObject scanner;

    public GameObject enemyParent;

    [Header("Animators")]
    public Animator cameraAnimator;
    public Animator slammedDoorAnimator;

    [Header("Fix Crouch Glitch")]
    private Transform camOriginalParent;
    private Vector3 camOriginalPosition;
    private Quaternion camOriginalRotation;
    private Transform cameraRig;

    private void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindWithTag("Player");

        scanner.SetActive(true);
        enemyParent.SetActive(false);
        isChasePlaying = false;
        cameraAnimator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            Debug.Log("Start chase");
            triggered = true;

            AudioClip banging = Resources.Load<AudioClip>("Sounds/trying-to-open-a-locked-door-104302");
            AudioManager.Instance.PlaySFX(banging);
            StartCutscene();
        }
    }

    private void StartCutscene()
    {
        isChasePlaying = true;

        Captions.Instance.TimedShowCaptions("What's that sound?", 3f);

        // Disable current player movements
        PlayerController.DisablePlayerControl();

        // Store original camera transform BEFORE reparenting
        camOriginalParent = mainCamera.transform.parent;
        camOriginalPosition = mainCamera.transform.position;
        camOriginalRotation = mainCamera.transform.rotation;

        // Create rig
        cameraRig = new GameObject("CameraRig").transform;
        cameraRig.position = camOriginalPosition;

        // Calculate the rotation offset so the camera faces world Y = -90
        Quaternion targetWorldRotation = Quaternion.Euler(0f, -90f, 0f);
        Quaternion offset = Quaternion.Inverse(camOriginalRotation) * targetWorldRotation;
        cameraRig.rotation = camOriginalRotation * offset;

        // Parent camera to rig
        mainCamera.transform.SetParent(cameraRig);

        // Enable animator on camera (same as before)
        cameraAnimator.enabled = true;

        // Reset animations
        ResetAnimations();

        // Play animation
        StartCoroutine(TurnAround());
    }



    private void ResetAnimations()
    {
        cameraAnimator.ResetTrigger("TurnAround");
        slammedDoorAnimator.ResetTrigger("SlamDoorOpen");
    }

    private IEnumerator TurnAround()
    {
        // Show enemy
        enemyParent.SetActive(true);
        enemyParent.GetComponent<EnemyChase>().enabled = false;

        // Play turn around animation
        cameraAnimator.SetTrigger("TurnAround");

        float turnAroundFirstHalfTime = 4f;
        yield return new WaitForSeconds(turnAroundFirstHalfTime);

        // Cult member appears
        slammedDoorAnimator.SetTrigger("SlamDoorOpen");
        AudioClip doorSlam = Resources.Load<AudioClip>("Sounds/door-slam-172171");
        AudioManager.Instance.PlaySFX(doorSlam, 2f);

        // Wait til it's done
        yield return new WaitForSeconds(7f - turnAroundFirstHalfTime);

        // Hide scanner
        scanner.SetActive(false);
        Destroy(cameraAnimator);

        Captions.Instance.TimedShowCaptions("Run ([W/A/S/D], SPACE, Left Ctrl) Away", 3f);

        PlayerController.EnablePlayerControl();
        PlayerController.RunningConstantly();

        // Restore camera to player
        mainCamera.transform.SetParent(camOriginalParent);
        mainCamera.transform.position = camOriginalPosition;
        //mainCamera.transform.rotation = camOriginalRotation;

        // Re-align camera height
        Vector3 camPos = mainCamera.transform.localPosition;
        camPos.y = PlayerController.Instance.GetCameraStandingHeight();
        mainCamera.transform.localPosition = camPos;

        //NewMovementSystem();

        enemyParent.GetComponent<EnemyChase>().enabled = true;

        // Music, SFX
        AudioClip music = Resources.Load<AudioClip>("Music/happy-halloween-169509");
        AudioManager.Instance.PlayMusic(music, true);
        AudioClip heartbeat = Resources.Load<AudioClip>("Sounds/tachycardic-heart-beat-417364_BG");
        AudioManager.Instance.PlayBackgroundHum(heartbeat);
    }

    private void NewMovementSystem()
    {
        mainCamera.transform.SetParent(null);
        mainCamera.gameObject.AddComponent<ChaseMovementSystem>(); 
    }
}
