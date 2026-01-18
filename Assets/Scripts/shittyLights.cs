using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
public class shittyLights : MonoBehaviour
{
    private Light light2;
    [SerializeField, Range(0f, 3f)] private float minIntensity = 0.1f;
    [SerializeField, Range(0f, 3f)] private float maxIntensity = 1.2f;
    [SerializeField, Range(0f, 3f)] float minTime;
    [SerializeField, Range(0f, 3f)] float maxTime;
    [SerializeField, Range(0f, 3f)] float minLightOut;
    [SerializeField, Range(0f, 3f)] float maxLightOut;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] AudioClip[] creepySounds;
    private bool lightOn=true;
    private float timer = 1f;

    //SPATIAL BLEND OF AUDIO SOURCE SHOULD BE 3D
    //max distance low, 10???
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (light2 == null)
        {
            light2 = GetComponent<Light>();
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        ValidateIntensityBounds();
    }
    private void ValidateIntensityBounds()
    {
        if (!(minIntensity > maxIntensity))
        {
            return;
        }
        Debug.LogWarning("Min Intensity is greater than max Intensity, Swapping values!");
        (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timer)
        {
            if (lightOn)
            {
                timer = Time.time + Random.Range(minLightOut, maxLightOut);
                audioSource.clip = creepySounds[UnityEngine.Random.Range(0, creepySounds.Length)];
                audioSource.Play();

            }
            else {
                timer = Time.time + Random.Range(minTime, maxTime);
            }
            light2.enabled = !light2.enabled;
            lightOn = !lightOn;



            //Debug.Log("erngiujewrg");
            //timer = 
        }
    }
}
