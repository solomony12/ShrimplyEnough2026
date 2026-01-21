using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;

public class QuicktimeEventTrigger : MonoBehaviour
{
    public enum Action
    {
        SlideLeft,
        SlideRight,
        Jump,
        Crouch,
        Door,
        TurnLeft,
        TurnRight
    }

    [Header("Action Bools")]
    private bool leftQuickTimeBool;
    private bool rightQuickTimeBool;
    private bool jumpQuickTimeBool;
    private bool crouchQuickTimeBool;
    private bool doorQuickTimeBool;
    private bool turnLeftQuickTimeBool;
    private bool turnRightQuickTimeBool;

    [Header("Timer")]
    private float quickTimeTimer = 0f;
    private bool quickTimeActive = false;
    private const float quickTimeDuration = 0.3f;

    [Header("Movement Settings")]
    [SerializeField] private float slideDistance = 2f;
    [SerializeField] private float slideDuration = 0.2f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float jumpDuration = 0.4f;
    [SerializeField] private float crouchHeight = 0.7f;
    [SerializeField] private float crouchDuration = 0.4f;
    [SerializeField] private float turnAngle = 90f;

    [SerializeField] private ChaseMovementSystem chaseMovementSystem;

    private void ResetQuickTimeBools()
    {
        leftQuickTimeBool = false;
        rightQuickTimeBool = false;
        jumpQuickTimeBool = false;
        crouchQuickTimeBool = false;
        doorQuickTimeBool = false;
        turnLeftQuickTimeBool = false;
        turnRightQuickTimeBool = false;
    }

    private Key GetPressedKey()
    {
        foreach (KeyControl key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
                return key.keyCode;
        }

        return Key.None;
    }

    public Action quickTimeEvent;

    /*
    private void OnTriggerEnter(Collider other)
    {
        ResetQuickTimeBools();

        if (other.CompareTag("Player"))
        {
            Debug.Log("Quick time!");

            quickTimeActive = true;
            quickTimeTimer = quickTimeDuration;

            switch (quickTimeEvent)
            {
                case Action.SlideLeft:
                    leftQuickTimeBool = true;
                    break;

                case Action.SlideRight:
                    rightQuickTimeBool = true;
                    break;

                case Action.Jump:
                    jumpQuickTimeBool = true;
                    break;

                case Action.Crouch:
                    crouchQuickTimeBool = true;
                    break;

                case Action.Door:
                    doorQuickTimeBool = true;
                    break;

                case Action.TurnLeft:
                    turnLeftQuickTimeBool = true;
                    break;

                case Action.TurnRight:
                    turnRightQuickTimeBool = true;
                    break;

                default:
                    Debug.LogWarning("Unknown quickTimeEvent enum: " + quickTimeEvent);
                    break;
            }

            Collider triggerZone = gameObject.GetComponent<Collider>();
            Destroy(triggerZone);
        }
    }

    
    private void Update()
    {
        if (!quickTimeActive)
            return;

        // Timer countdown
        quickTimeTimer -= Time.deltaTime;

        if (quickTimeTimer <= 0f)
        {
            Debug.Log("Quicktime failed: time ran out!");
            chaseMovementSystem.TriggerSlowDown();
            ResetQuickTimeBools();
            quickTimeActive = false;
            return;
        }

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Key pressedKey = GetPressedKey();

            if (leftQuickTimeBool && pressedKey == Key.A)
            {
                SlideLeft();
                QuickTimeSuccess();
            }
            else if (rightQuickTimeBool && pressedKey == Key.D)
            {
                SlideRight();
                QuickTimeSuccess();
            }
            else if (jumpQuickTimeBool && pressedKey == Key.Space)
            {
                Jump();
                QuickTimeSuccess();
            }
            else if (crouchQuickTimeBool && pressedKey == Key.LeftCtrl)
            {
                Crouch();
                QuickTimeSuccess();
            }
            else if (doorQuickTimeBool && pressedKey == Key.E)
            {
                OpenDoor();
                QuickTimeSuccess();
            }
            else if (turnLeftQuickTimeBool && pressedKey == Key.A)
            {
                TurnLeft();
                QuickTimeSuccess();
            }
            else if (turnRightQuickTimeBool && pressedKey == Key.D)
            {
                TurnRight();
                QuickTimeSuccess();
            }
        }
    }
    */

    private void QuickTimeSuccess()
    {
        Debug.Log("Quicktime success!");
        ResetQuickTimeBools();
        quickTimeActive = false;
    }

    // ----------------------
    // Quicktime Actions
    // ----------------------

    private void SlideLeft()
    {
        Transform cam = Camera.main.transform;
        Vector3 leftDir = -cam.right;
        Vector3 target = cam.position + leftDir * slideDistance;
        StartCoroutine(MoveOverTime(cam, target, slideDuration));
    }

    private void SlideRight()
    {
        Transform cam = Camera.main.transform;
        Vector3 rightDir = cam.right;
        Vector3 target = cam.position + rightDir * slideDistance;
        StartCoroutine(MoveOverTime(cam, target, slideDuration));
    }

    private void Jump()
    {
        Transform cam = Camera.main.transform;
        Vector3 start = cam.position;
        Vector3 peak = start + Vector3.up * jumpHeight;
        StartCoroutine(JumpOverTime(cam, start, peak, jumpDuration));
    }

    private void Crouch()
    {
        Transform cam = Camera.main.transform;
        Vector3 start = cam.position;
        Vector3 crouchPos = start - Vector3.up * crouchHeight;
        StartCoroutine(CrouchOverTime(cam, start, crouchPos, crouchDuration));
    }

    private void OpenDoor()
    {
        // Intentionally left empty
    }

    private void TurnLeft()
    {
        Transform cam = Camera.main.transform;
        cam.Rotate(Vector3.up, -turnAngle);
    }

    private void TurnRight()
    {
        Transform cam = Camera.main.transform;
        cam.Rotate(Vector3.up, turnAngle);
    }

    // ----------------------
    // Coroutines
    // ----------------------

    private System.Collections.IEnumerator MoveOverTime(Transform t, Vector3 target, float duration)
    {
        Vector3 start = t.position;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            t.position = Vector3.Lerp(start, target, timer / duration);
            yield return null;
        }

        t.position = target;
    }

    private System.Collections.IEnumerator JumpOverTime(Transform t, Vector3 start, Vector3 peak, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            // Smooth parabola motion
            float height = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            t.position = start + Vector3.up * height;

            yield return null;
        }

        t.position = start;
    }

    private System.Collections.IEnumerator CrouchOverTime(Transform t, Vector3 start, Vector3 crouchPos, float duration)
    {
        float half = duration / 2f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (timer < half)
                t.position = Vector3.Lerp(start, crouchPos, timer / half);
            else
                t.position = Vector3.Lerp(crouchPos, start, (timer - half) / half);

            yield return null;
        }

        t.position = start;
    }
}
