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

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction crawlAction;
    private InputAction sprintAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public float CrawlValue { get; private set; }

    public float SprintValue { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

        crawlAction.performed += context => CrawlValue = context.ReadValue<float>();
        crawlAction.canceled += context => CrawlValue = 0f;

        sprintAction.performed += context => SprintValue = context.ReadValue<float>();
        sprintAction.canceled += context => SprintValue = 0f;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        crawlAction.Enable();
        sprintAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        crawlAction.Disable();
        sprintAction.Disable();
    }

}
