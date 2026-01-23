using System;
using UnityEngine;

public class VoicelineTrigger : MonoBehaviour
{
    public AudioClip voiceline;
    public string captions;
    public float time;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        try
        {
            Captions.Instance.TimedShowCaptions(captions, time);
            AudioManager.Instance.PlayVoice(voiceline);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        Destroy(gameObject);
    }
}
