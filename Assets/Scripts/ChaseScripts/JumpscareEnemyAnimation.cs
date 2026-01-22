using UnityEngine;
using System.Collections;

public class JumpscareEnemyAnimation : MonoBehaviour
{
    public GameObject enemyJumpscareModel;
    private float shakeDuration = 2f;
    private float shakeMagnitude = 0.01f;

    private Vector3 originalPosition;

    private static JumpscareEnemyAnimation instance;

    private void Awake()
    {
        instance = this;

        if (enemyJumpscareModel != null)
        {
            enemyJumpscareModel.SetActive(false);
        }
    }

    public static void TriggerJumpscare()
    {
        if (instance != null)
        {
            instance.StartJumpscare();
        }
        else
        {
            Debug.LogWarning("JumpscareEnemyAnimation instance not found in scene.");
        }
    }

    private void StartJumpscare()
    {
        if (enemyJumpscareModel == null) return;

        enemyJumpscareModel.SetActive(true);
        originalPosition = enemyJumpscareModel.transform.localPosition;

        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float x = Mathf.Sin(elapsed * 50f) * shakeMagnitude;
            float y = Mathf.Sin(elapsed * 45f) * shakeMagnitude;

            enemyJumpscareModel.transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            yield return null;
        }

        enemyJumpscareModel.transform.localPosition = originalPosition;
    }
}
