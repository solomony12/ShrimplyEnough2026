using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareTrigger : MonoBehaviour
{

    public int methodToTriggerIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Boo!");

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

}
