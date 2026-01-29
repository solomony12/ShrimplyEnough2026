using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    [Header("Door Interaction")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private Key interactKey = Key.E;

    private Camera mainCamera;

    private HoverCaptions hoverCaptions;

    private void Awake()
    {
        mainCamera = Camera.main;
        hoverCaptions = HoverCaptions.Instance;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        bool isHovering = false;

        // HOVERING
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Door
            if (hit.collider.CompareTag("Door"))
            {
                Door door = hit.collider.GetComponentInParent<Door>();

                // If door exists and is not locked
                if (door != null && !door.locked)
                {
                    isHovering = true;
                    hoverCaptions.ShowCaptions("Press [E] to interact");

                    // Interact
                    if (Keyboard.current[interactKey].wasPressedThisFrame)
                    {
                        TryInteract();
                    }
                }
                else
                {
                    // Optionally, clear hover text if locked
                    isHovering = false;
                    hoverCaptions.HideCaptions();
                }
            }
            // Level Door
            else if (hit.collider.CompareTag("LevelDoor"))
            {
                isHovering = true;
                hoverCaptions.ShowCaptions("Press [E] to proceed");
                // Interact
                if (Keyboard.current[interactKey].wasPressedThisFrame && !WarningSceneScript.isWarningScreenUp)
                {
                    TryInteract();
                }
            }
            // Keycard
            else if (hit.collider.CompareTag("Keycard"))
            {
                isHovering = true;
                hoverCaptions.ShowCaptions("Press [E] to pick up");
                // Interact
                if (Keyboard.current[interactKey].wasPressedThisFrame)
                {
                    KeycardWarningSystem keycardSystem = hit.collider.GetComponentInParent<KeycardWarningSystem>();
                    keycardSystem.PickUpKeycard();
                }
            }
            // Starting Cutout
            else if (hit.collider.CompareTag("StartingCutout"))
            {
                isHovering = true;
                hoverCaptions.ShowCaptions("Press [E] to interact");
                // Interact
                if (Keyboard.current[interactKey].wasPressedThisFrame)
                {
                    try
                    {
                        Captions.Instance.TimedShowCaptions("Bubble... That's a dumb name for a shrimp mascot.", 5f);
                        //AudioClip line = Resources.Load<AudioClip>("Voicelines/main");
                        //AudioManager.Instance.PlayVoice(line);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
            }
            // Flashlight
            else if (hit.collider.CompareTag("Flashlight"))
            {
                isHovering = true;
                hoverCaptions.ShowCaptions("Press [E] to pick up");
                // Interact
                if (Keyboard.current[interactKey].wasPressedThisFrame)
                {
                    Transform parent = hit.collider.transform.parent;

                    if (parent != null)
                    {
                        foreach (Transform child in parent)
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                    Flashlight.Instance.canUseFlashlight = true;
                    Captions.Instance.TimedShowCaptions("[C] to toggle the flashlight", 6f);
                }
            }
        }

        if (!isHovering)
        {
            hoverCaptions.HideCaptions();
        }

    }


    private void TryInteract()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                Door door = hit.collider.GetComponent<Door>();

                if (door != null)
                {
                    Debug.Log("Toggle door");
                    door.ToggleDoor();
                }
            }
            if (hit.collider.CompareTag("LevelDoor"))
            {
                ChangeLevel door = hit.collider.GetComponent<ChangeLevel>();

                if (door != null)
                {
                    if (KeycardWarningSystem.CanUnlockDoor())
                    {
                        WarningSceneScript.isWarningScreenUp = true;
                        Cursor.lockState = CursorLockMode.None;
                        PlayerController.DisablePlayerControl();
                        WarningSceneScript.previousSceneName = SceneManager.GetActiveScene().name;
                        SceneManager.LoadScene("WarningScene", LoadSceneMode.Additive);
                    }
                    else
                    {
                        Captions.Instance.TimedShowCaptions("I need to find the keycard", 3f);
                    }
                }
            }
        }
    }
}
