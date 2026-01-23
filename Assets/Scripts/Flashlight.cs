using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public static Flashlight Instance;

    public GameObject flashlightObject;
    public Light flashlightLight;

    private bool _canUseFlashlight;

    [SerializeField] private Key interactKey = Key.C;

    private bool lightsOn;

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

        _canUseFlashlight = false;
        flashlightLight.enabled = true;
        flashlightObject.SetActive(false);
        lightsOn = true;
    }

    public bool canUseFlashlight
    {
        get => _canUseFlashlight;
        set
        {
            if (_canUseFlashlight == value) return; // no change

            _canUseFlashlight = value;
            OnIsOnChanged();
        }
    }

    void OnIsOnChanged()
    {
        Debug.Log("Flashlight updated to: " + _canUseFlashlight);

        flashlightObject.SetActive(_canUseFlashlight);
        
    }

    private void Update()
    {
        if (!_canUseFlashlight)
            return;

        if (Keyboard.current[interactKey].wasPressedThisFrame && !WarningSceneScript.isWarningScreenUp)
        {
            // Switch current state of light
            lightsOn = !lightsOn;
            flashlightLight.enabled = lightsOn;
        }
    }



}
