using Unity.Mathematics;
using UnityEngine;

public class noises : MonoBehaviour
{
    private float timeTillSound = 100f;
    [SerializeField] float minimumTime = 100f;
    [SerializeField] float maximumTime = 200f;
    [SerializeField] AudioClip[] creepySounds;
    [SerializeField] AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeTillSound)
        {
            timeTillSound = Time.time + UnityEngine.Random.Range(minimumTime, maximumTime);
            audioSource.clip = creepySounds[UnityEngine.Random.Range(0, creepySounds.Length)];
            audioSource.Play();
            //audioSource.PlayDelayed
        }
    }
}
