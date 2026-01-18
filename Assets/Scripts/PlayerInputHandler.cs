using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerInputHandler : MonoBehaviour
{

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Axction Map Name References")]
    [SerializeField]  private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string crawl = "Crawl";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string itemUp = "ItemUp";
    [SerializeField] private string itemDown = "ItemDown";
    [SerializeField] private string itemSelect = "ItemSelect";
    [SerializeField] private string escape = "Escape";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction crawlAction;
    private InputAction sprintAction;
    private InputAction interactAction;
    private InputAction itemUpAction;
    private InputAction itemDownAction;
    private InputAction itemSelectAction;
    private InputAction escapeAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool CrawlTriggered { get; private set; }
    public float CrawlValue { get; private set; }
    public float SprintValue { get; private set; }
    public bool InteractTriggered { get; private set; }
    public bool ItemUpTriggered { get; private set; }
    public bool ItemDownTriggered { get; private set; }
    public bool ItemSelectTriggered { get; private set; }
    public bool EscapeTriggered { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

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

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        crawlAction = playerControls.FindActionMap(actionMapName).FindAction(crawl);
        sprintAction = playerControls.FindActionMap(actionMapName).FindAction(sprint);
        interactAction = playerControls.FindActionMap(actionMapName).FindAction(interact);
        itemUpAction = playerControls.FindActionMap(actionMapName).FindAction(itemUp);
        itemDownAction = playerControls.FindActionMap(actionMapName).FindAction(itemDown);
        itemSelectAction = playerControls.FindActionMap(actionMapName).FindAction(itemSelect);
        escapeAction = playerControls.FindActionMap(actionMapName).FindAction(escape);
        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        moveAction.performed += contect => MoveInput = contect.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        lookAction.performed += contect => LookInput = contect.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = Vector2.zero;

        jumpAction.performed += context => JumpTriggered = true;
        jumpAction.canceled += context => JumpTriggered = false;

        crawlAction.performed += context => CrawlTriggered = true;
        crawlAction.performed += context => CrawlValue = context.ReadValue<float>();
        crawlAction.canceled += context => CrawlTriggered = false;
        crawlAction.canceled += context => CrawlValue = 0f;

        sprintAction.performed += context => SprintValue = context.ReadValue<float>();
        sprintAction.canceled += context => SprintValue = 0f;

        interactAction.performed += context => InteractTriggered = true;
        interactAction.canceled += context => InteractTriggered = false;

        itemUpAction.performed += context => ItemUpTriggered = true;
        itemUpAction.canceled += context => ItemUpTriggered = false;

        itemDownAction.performed += context => ItemDownTriggered = true;
        itemDownAction.canceled += context => ItemDownTriggered = false;

        itemSelectAction.performed += context => ItemSelectTriggered = true;
        itemSelectAction.canceled += context => ItemSelectTriggered = false;

        escapeAction.performed += context => EscapeTriggered = true;
        escapeAction.canceled += context => EscapeTriggered = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        crawlAction.Enable();
        sprintAction.Enable();
        interactAction.Enable();
        itemUpAction.Enable();
        itemDownAction.Enable();
        itemSelectAction.Enable();
        escapeAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        crawlAction.Disable();
        sprintAction.Disable();
        interactAction.Disable();
        itemUpAction.Disable();
        itemDownAction.Disable();
        itemSelectAction.Disable();
        escapeAction.Disable();
    }
}
