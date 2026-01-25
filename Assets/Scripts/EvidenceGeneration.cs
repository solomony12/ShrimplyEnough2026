using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EvidenceGeneration : MonoBehaviour
{
    public static EvidenceGeneration Instance;

    public static int CorrectEvidenceAmount;

    private static float CurrentThresholdPercentage;

    private static float PercentThreshold = 85.0f; // TODO: 85.0

    public GameObject percentageTextGameObject;

    private static TMP_Text percentageText;

    private const string evidenceText = "Investigation Process:";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        percentageTextGameObject = GameObject.FindWithTag("EvidenceText");
        percentageText = percentageTextGameObject.GetComponent<TMP_Text>();

        ResetEvidenceCounter();
    }

    public void ShowText()
    {
        percentageTextGameObject.SetActive(true);
    }

    public void HideText()
    {
        percentageTextGameObject.SetActive(false);
    }

    public static void ResetEvidenceCounter()
    {
        Debug.Log("RESET EVIDENCE COUNTER");
        CorrectEvidenceAmount = 0;
        CurrentThresholdPercentage = 0;
        if (percentageText == null)
            throw new System.Exception("FAILED TO RESET TEXT");
        percentageText.text = $"{evidenceText} {CurrentThresholdPercentage:0.00}%";
    }

    /// <summary>
    /// Adds percent amount (using floats)
    /// </summary>
    /// <param name="percent">Amount to add to evidence percentage</param>
    /// <returns>True if we've reached the threshold</returns>
    public static bool AddPercentage(float percent)
    {
        CurrentThresholdPercentage += percent;
        percentageText.text = $"{evidenceText} {CurrentThresholdPercentage:0.00}%";

        // If we've reached the threshold
        return CurrentThresholdPercentage >= PercentThreshold;
    }
}
