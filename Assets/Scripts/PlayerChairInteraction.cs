using UnityEngine;

public class PlayerChairInteraction : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Chair"))
            return;

        ChairSwivel chair = hit.collider.GetComponent<ChairSwivel>();
        if (chair == null)
            return;

        chair.SwivelFromHit(transform.position);
    }
}
