using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScanEvidence : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject player;

    private PlayerInputHandler inputHandler;
    private HoverCaptions evidenceCaptions;
    private IdentificationSystem identificationSystem;

    [Header("Distane to Scan")]
    private float maxPlayerObjectDistance = 5f;
    private float maxCameraObjectDistance = 3f;

    [Header("Scanner Variables")]
    [SerializeField] private float holdTimeToScan = 2f;
    private float holdTimer = 0f;
    private GameObject currentEvidence = null;

    public static bool IsDisplayOpen = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        player = GameObject.FindWithTag("Player");
        evidenceCaptions = HoverCaptions.Instance;
        inputHandler = PlayerInputHandler.Instance;
        identificationSystem = IdentificationSystem.Instance;

        if (inputHandler == null)
            Debug.LogError("PlayerInputHandler.Instance is NULL!");

        if (evidenceCaptions == null)
            Debug.LogError("HoverCaptions.Instance is NULL!");
    }

    void Update()
    {
        HandleScan();
    }

    void HandleScan()
    {
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
            evidenceCaptions.HideCaptions();
            return;
        }

        // HOLD logic
        if (inputHandler.ScanTriggered)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTimeToScan)
            {
                SelectedEvidence(currentEvidence);
                holdTimer = 0f;
            }
        }
        else
        {
            // Player released hold
            holdTimer = 0f;
        }
    }

    private void SelectedEvidence(GameObject evidence)
    {
        if (IsDisplayOpen) return;

        IsDisplayOpen = true;

        Debug.Log("Evidence scanned: " + evidence.name);

        // Add scanning logic here
        // Call EvidenceManager to remove this evidence trigger from scene and update generation
        // Call IdentificationSystem
        StartCoroutine(identificationSystem.StartDisplaySystem(evidence.name));

        // Make sure the evidence can't be re-accessed
        evidence.tag = "Untagged";
    }
}
