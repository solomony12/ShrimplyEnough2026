using UnityEngine;

public class KeycardWarningSystem : MonoBehaviour
{

    public GameObject keycard;
    public GameObject changeLevelDoor;

    private static bool pickedUpKeycard;

    public static KeycardWarningSystem Instance;

    private AudioClip keycardPickedUpClip;

    void Awake()
    {
        pickedUpKeycard = false;
        keycard.SetActive(true);

        keycardPickedUpClip = Resources.Load<AudioClip>("Sounds/correct-156911");
    }

    public void PickUpKeycard()
    {
        AudioManager.Instance.PlaySFX(keycardPickedUpClip);
        pickedUpKeycard = true;
        keycard.SetActive(false);
    }

    public static bool CanUnlockDoor()
    {
        return pickedUpKeycard;
    }

    public int GetLevelToLoadIndex()
    {
        return changeLevelDoor.GetComponent<ChangeLevel>().levelIndex;
    }
}
