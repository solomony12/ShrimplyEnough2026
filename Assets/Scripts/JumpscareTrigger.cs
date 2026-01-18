using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Boo!");
            Destroy(gameObject);
        }
    }
}
