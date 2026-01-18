using UnityEngine;

public class EvidenceGeneration : MonoBehaviour
{
    public static EvidenceGeneration Instance;

    public static int CorrectEvidenceAmount;

    private int currentThresholdLevel;

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

        CorrectEvidenceAmount = 0;
    }

    private void UpdateEvidenceThresholds()
    {
        // 7 threshold levels (starting at 0)
        int evidenceInEachSection = 5;
        int thresholdLevels = 6; // x + 1 (due to starting at 0)
        int currentThresholdLevel = CorrectEvidenceAmount / evidenceInEachSection;
        if (currentThresholdLevel > thresholdLevels) currentThresholdLevel = thresholdLevels;
    }

    /// <summary>
    /// When a new section is loaded, generate evidence
    /// </summary>
    public void GenerateEvidence()
    {
        // Get the game objects of that section
        // Set the names of those game objects based on threshold levels (Evidence00, Evidence08, etc)
    }
}
