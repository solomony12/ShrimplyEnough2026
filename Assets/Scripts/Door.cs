using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Hinge")]
    [SerializeField] private Transform hinge;

    [Header("Rotation")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 4f;

    [Header("One Way Door")]
    [SerializeField] private bool oneWay = false;

    private bool isOpen = false;
    private bool locked = false;

    private Quaternion closedRot;
    private Quaternion openRot;

    private void Awake()
    {
        if (hinge == null)
            hinge = transform;

        closedRot = hinge.localRotation;
        openRot = closedRot * Quaternion.Euler(0, openAngle, 0);
    }

    private void Update()
    {
        Quaternion target = isOpen ? openRot : closedRot;
        hinge.localRotation = Quaternion.Lerp(hinge.localRotation, target, Time.deltaTime * openSpeed);
    }

    public void ToggleDoor()
    {
        if (locked) return;   // locked doors can't open

        isOpen = !isOpen;
    }

    public void CloseAndLock()
    {
        isOpen = false;
        locked = true;
    }

    public bool IsOneWay()
    {
        return oneWay;
    }
}
