using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScanEvidence : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject player;
    private GameObject scanner;

    [Header("Scanner Info")]
    private Vector3 scannerOrigPos = new Vector3(0.47f, -0.362f, 0.748f);
    private Vector3 scannerOrigRot = Vector3.zero;
    private Vector3 scannerNewPos = new Vector3(0.056694299f, -0.383044481f, 0.784358799f);
    private Vector3 scannerNewRot = new Vector3(12.8708248f, 17.9218407f, 359.800537f);
    [SerializeField] private float scannerLerpSpeed = 8f;
    private bool isScanning = false;

    private PlayerInputHandler inputHandler;
    private HoverCaptions evidenceCaptions;
    private IdentificationSystem identificationSystem;

    [Header("Distance to Scan")]
    private float maxPlayerObjectDistance = 5f;
    private float maxCameraObjectDistance = 3f;

    [Header("Scanner Variables")]
    [SerializeField] private float holdTimeToScan = 2f;
    private float holdTimer = 0f;
    private GameObject currentEvidence = null;

    public static bool IsDisplayOpen = false;

    [Header("Audio")]
    [SerializeField] private AudioClip scannedClip;
    [SerializeField] private AudioClip scanningClip;
    [SerializeField] private float scanningPlayInterval = 0.5f;
    private float scanningCooldown = 0f;

    private void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindWithTag("Player");
        scanner = GameObject.FindWithTag("Scanner");
        scanner.transform.position = scannerOrigPos;
        scanner.transform.rotation = Quaternion.Euler(scannerOrigRot);
        evidenceCaptions = HoverCaptions.Instance;
        inputHandler = PlayerInputHandler.Instance;
        identificationSystem = IdentificationSystem.Instance;

        if (inputHandler == null)
            Debug.LogError("PlayerInputHandler.Instance is NULL!");

        if (evidenceCaptions == null)
            Debug.LogError("HoverCaptions.Instance is NULL!");

        scannedClip = Resources.Load<AudioClip>("Sounds/correct-156911");
        scanningClip = Resources.Load<AudioClip>("Sounds/doorscan-102283");
    }

    void Update()
    {
        HandleScan();
        HandleScannerLerp();
    }

    void HandleScan()
    {
        if (IsDisplayOpen || SettingsMenuUI.SettingsIsOpen)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        bool isHoveringEvidence = false;

        // Close enough to player
        if (Physics.Raycast(ray, out hit, maxPlayerObjectDistance))
        {
            // Evidence
            if (hit.collider.CompareTag("Evidence"))
            {
                float dist = Vector3.Distance(player.transform.position, hit.collider.transform.position);

                // Close enough for camera to see
                if (dist <= maxCameraObjectDistance)
                {
                    //Debug.Log("Hovering over evidence: " + hit.collider.name);

                    isHoveringEvidence = true;
                    currentEvidence = hit.collider.gameObject;

                    evidenceCaptions.ShowCaptions("Hold 'Left Click' to scan evidence");
                }
            }
        }

        if (!isHoveringEvidence)
        {
            currentEvidence = null;
            holdTimer = 0f;
            isScanning = false;
            //evidenceCaptions.HideCaptions();
            return;
        }

        // HOLD logic
        if (inputHandler.ScanTriggered)
        {
            isScanning = true;
            holdTimer += Time.deltaTime;

            scanningCooldown -= Time.deltaTime;
            if (scanningCooldown <= 0f)
            {
                AudioManager.Instance.PlaySFX(scanningClip, 2f);
                scanningCooldown = scanningPlayInterval;
            }

            if (holdTimer >= holdTimeToScan)
            {
                SelectedEvidence(currentEvidence);
                holdTimer = 0f;
                isScanning = false;
            }
        }
        else
        {
            // Player released hold
            isScanning = false;
            holdTimer = 0f;
            scanningCooldown = 0f;
        }

    }

    void HandleScannerLerp()
    {
        if (scanner == null) return;

        Vector3 targetPos = isScanning ? scannerNewPos : scannerOrigPos;
        Quaternion targetRot = Quaternion.Euler(isScanning ? scannerNewRot : scannerOrigRot);

        scanner.transform.localPosition = Vector3.Lerp(
            scanner.transform.localPosition,
            targetPos,
            Time.deltaTime * scannerLerpSpeed
        );

        scanner.transform.localRotation = Quaternion.Lerp(
            scanner.transform.localRotation,
            targetRot,
            Time.deltaTime * scannerLerpSpeed
        );
    }

    private void SelectedEvidence(GameObject evidence)
    {
        if (IsDisplayOpen) return;

        IsDisplayOpen = true;

        AudioManager.Instance.PlaySFX(scannedClip);

        evidenceCaptions.HideCaptions();

        Debug.Log("Evidence scanned: " + evidence.name);

        // Add scanning logic here
        // Call EvidenceManager to remove this evidence trigger from scene and update generation
        // Call IdentificationSystem
        StartCoroutine(identificationSystem.StartDisplaySystem(evidence.name));

        // Make sure the evidence can't be re-accessed
        evidence.tag = "Untagged";
    }
}
