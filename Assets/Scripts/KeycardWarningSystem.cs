using UnityEngine;

public class KeycardWarningSystem : MonoBehaviour
{

    public GameObject keycard;
    public GameObject changeLevelDoor;

    private bool pickedUpKeycard;

    void Awake()
    {
        pickedUpKeycard = false;
        keycard.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
