using UnityEngine;

public class CardboardCutoutJumpscare : MonoBehaviour
{
    public GameObject cutout;
    public Light light;

    private void Awake()
    {
        cutout.SetActive(false);
        light.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        cutout.SetActive(true);
        light.enabled = true;
    }
}
