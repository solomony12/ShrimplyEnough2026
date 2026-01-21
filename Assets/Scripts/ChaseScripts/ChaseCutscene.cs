using System.Collections;
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

    private void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindWithTag("Player");

        scanner.SetActive(true);
        enemyParent.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            Debug.Log("Start chase");
            triggered = true;
            StartCutscene();
        }
    }

    private void StartCutscene()
    {
        isChasePlaying = true;

        Captions.Instance.TimedShowCaptions("What's that sound?", 3f);

        // Disable current player movements
        PlayerController.DisablePlayerControl();

        // Camera rot
        Quaternion wherePlayerWasLooking = mainCamera.transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(0f, 0.0f, 0f);
        mainCamera.transform.localRotation = Quaternion.Lerp(wherePlayerWasLooking, targetRot, 1f);

        // Camera pos
        Vector3 startingCamPos = mainCamera.transform.localPosition;
        Vector3 targetCamPos = new Vector3(0f, 0.76f, 0f);
        cameraAnimator.transform.localPosition = Vector3.Lerp(startingCamPos, targetCamPos, 1f);

        // Player
        Vector3 targetPos = new Vector3(-4.76927042f, 1.08f, -1.95f);
        player.transform.position = Vector3.Lerp(player.transform.position, targetPos, 1f);

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

        // Wait til it's done
        yield return new WaitForSeconds(7f - turnAroundFirstHalfTime);

        // Hide scanner
        scanner.SetActive(false);
        Destroy(cameraAnimator);

        Captions.Instance.TimedShowCaptions("Run", 3f);

        PlayerController.EnablePlayerControl();
        PlayerController.RunningConstantly();
        //NewMovementSystem();

        enemyParent.GetComponent<EnemyChase>().enabled = true;
    }

    private void NewMovementSystem()
    {
        mainCamera.transform.SetParent(null);
        mainCamera.gameObject.AddComponent<ChaseMovementSystem>(); 
    }
}
