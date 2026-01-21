using UnityEngine;

public class dumassSplineHelper : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float timebetween = 1f;
    [SerializeField] int duration = 100;
    [SerializeField] GameObject toSpawn;
    [SerializeField] GameObject secondarySpawn;
    [SerializeField] int secondarySpawnFrequency;
    private int num = 0;
    private float timer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timebetween && num<duration)
        {
            if(num%secondarySpawnFrequency == 0)
            {
                GameObject spawnedObject = Instantiate(secondarySpawn, transform.position, transform.rotation);
            }
            else
            {
                GameObject spawnedObject = Instantiate(toSpawn, transform.position, transform.rotation);
            }

                timer = 0;
            num++;
        }
    }
}
