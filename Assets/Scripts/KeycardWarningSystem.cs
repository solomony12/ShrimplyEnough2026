using UnityEngine;

public class KeycardWarningSystem : MonoBehaviour
{

    public GameObject keycard;
    public GameObject changeLevelDoor;

    private static bool pickedUpKeycard;

    public static KeycardWarningSystem Instance;

    void Awake()
    {
        pickedUpKeycard = false;
        keycard.SetActive(true);
    }

    public void PickUpKeycard()
    {
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
