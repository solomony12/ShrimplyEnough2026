using UnityEngine;

public class QuicktimeEventTrigger : MonoBehaviour
{
    public enum Action
    {
        SlideLeft,
        SlideRight,
        Jump,
        Crouch,
        Door
    }

    public Action quickTimeEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Quick time!");

            // STUFF HERE
            switch (quickTimeEvent)
            {
                case Action.SlideLeft:
                    break;

                case Action.SlideRight:
                    break;

                case Action.Jump:
                    break;

                case Action.Crouch:
                    break;

                case Action.Door:
                    break;

                default:
                    Debug.LogWarning("Unknown quickTimeEvent enum: " + quickTimeEvent);
                    break;
            }

            //Destroy(gameObject);
        }
    }
}
