using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class JumpscareTrigger : MonoBehaviour
{

    public int methodToTriggerIndex;

    [Header("End Cutscene")]
    public PlayableDirector director;
    private Vector3 startPointPos = new Vector3(52f, -3.6f, 94f);
    private Quaternion startPointRot = Quaternion.Euler(0f, 325f, 0f);
    public float moveDuration = 3.0f;
    public static bool isCutscenePlaying = false;

    private void Awake()
    {
        isCutscenePlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Boo!");
            GetComponent<Collider>().enabled = false;

            // STUFF HERE
            switch (methodToTriggerIndex)
            {
                case 4:
                    OfficeLightsOff();
                    break;

                // Vent to final area
                case 7:
                    Debug.Log("Going to final area");
                    ChaseCutscene.isChasePlaying = false;
                    SceneManager.LoadScene("7_EndingLevel");
                    break;

                // Ending Cutscene
                case 100:
                    Debug.Log("Playing end cutscene");
                    AudioManager.Instance.StopBackgroundHum();
                    isCutscenePlaying = true;
                    StartCoroutine(MovePlayerToStart(other.transform));
                    break;

                default:
                    Debug.LogWarning("Unknown trigger index: " + methodToTriggerIndex);
                    break;
            }

            //Destroy(gameObject);
        }
    }

    public void OfficeLightsOff()
    {
        StartCoroutine(OfficeLightsOffRoutine());
    }

    private IEnumerator OfficeLightsOffRoutine()
    {
        Light directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

        shittyLights flicker = directionalLight.GetComponent<shittyLights>();

        if (flicker == null)
        {
            flicker = directionalLight.gameObject.AddComponent<shittyLights>();
        }

        flicker.minLightOut = 0.05f;
        flicker.maxLightOut = 0.15f;
        flicker.minTime = 0.02f;
        flicker.maxTime = 0.1f;
        flicker.minIntensity = 0.05f;
        flicker.maxIntensity = 1.2f;

        yield return new WaitForSeconds(2f);

        Destroy(flicker);

        directionalLight.enabled = false;

        Destroy(gameObject);
    }

    // ENDING CUTSCENE STUFF

    IEnumerator MovePlayerToStart(Transform player)
    {
        // Disable gameplay control
        PlayerController.DisablePlayerControl();

        // Player
        Vector3 startPos = player.position;
        Quaternion startRot = player.rotation;

        // Camera
        Camera camera = Camera.main;
        Vector3 camStartPos = camera.transform.localPosition;
        Quaternion camStartRot = camera.transform.localRotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;

            player.position = Vector3.Lerp(startPos, startPointPos, t);
            player.rotation = Quaternion.Slerp(startRot, startPointRot, t);

            camera.transform.localPosition = Vector3.Lerp(camStartPos, new Vector3(0f, 0.76f, 0f), t);
            camera.transform.localRotation = Quaternion.Slerp(camStartRot, Quaternion.Euler(0f, 0f, 0f), t);

            yield return null;
        }

        // Snap cleanly
        player.position = startPointPos;
        player.rotation = startPointRot;
        camera.transform.localPosition = camStartPos;
        camera.transform.localRotation= camStartRot;

        // NOW play timeline
        director.time = 0;
        director.Play();
    }

    private void OnEnable()
    {
        director.stopped += OnTimelineStopped;
    }

    private void OnDisable()
    {
        director.stopped -= OnTimelineStopped;
    }

    private void OnTimelineStopped(PlayableDirector d)
    {
        if (d == director)
        {
            StartCoroutine(GoToMain());
        }
    }

    private IEnumerator GoToMain()
    {
        yield return new WaitForSeconds(3f);
        isCutscenePlaying = false;
        Cursor.lockState = CursorLockMode.None;
        SceneTransition.Instance.StartTransition("MainMenu");
    }
}
