using UnityEngine;
using UnityEngine.InputSystem;

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
                isHovering = true;
                hoverCaptions.ShowCaptions("Press [E] to interact");
                // Interact
                if (Keyboard.current[interactKey].wasPressedThisFrame)
                {
                    TryInteract();
                }
            }
            // Level Door
            else if (hit.collider.CompareTag("LevelDoor"))
            {
                isHovering = true;
                hoverCaptions.ShowCaptions("Press [E] to proceed");
                // Interact
                if (Keyboard.current[interactKey].wasPressedThisFrame)
                {
                    TryInteract();
                }
            }
            // Keycard
            else if (hit.collider.CompareTag("Keycard"))
            {
                isHovering = true;
                hoverCaptions.ShowCaptions("Press [E] to pick up");
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
                    door.MoveToNextLevel();
                }
            }
        }
    }
}
