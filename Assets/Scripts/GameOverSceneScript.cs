using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverSceneScript : MonoBehaviour
{
    public TMP_Text displayText;
    private string sceneToLoad;

    string[] safetyQuotes = new string[] {
        "Safety is not a feeling. It is a protocol.",
        "If it can be measured, it can be mitigated.",
        "Unknown substances do not excuse unknown outcomes.",
        "Risk ignored is risk multiplied.",
        "Every object has a classification. Every classification has consequences.",
        "OSHRIMP does not eliminate danger. It manages it.",
        "Compliance is the first layer of protection.",
        "You are safest when you follow procedure.",
        "Investigation precedes mitigation.",
        "Health and safety begin with accurate identification.",
        "Ambiguity is a hazard.",
        "Risk assessment is not optional.",
        "Mitigation delayed is mitigation denied.",
        "Our responsibility ends only when the threat is documented.",
        "No object is harmless until OSHRIMP says it is.",
        "The safest outcome is the most controlled one.",
        "Safety improves when variables are reduced.",
        "Not all risks require public disclosure.",
        "An object’s history is less important than its current behavior.",
        "Compliance ensures continuity.",
        "Incidents are opportunities for policy clarification.",
        "Employee discretion is encouraged within defined limits.",
        "Some classifications remain internal for efficiency.",
        "The absence of evidence does not indicate the absence of risk.",
        "Safety thresholds may vary depending on operational necessity.",
        "Risk mitigation does not guarantee comfort.",
        "Public reassurance is a component of hazard control."
    };

    private void Awake()
    {
        if (displayText == null)
        {
            Debug.LogError("TMP_Text is not assigned!");
            return;
        }

        // Reset chase scene things
        ChaseCutscene.isChasePlaying = false;
        PlayerController.ResetRunningConstantly();

        // For now, just reload lvl6
        sceneToLoad = "6_FinalArea";

        // Pick random message
        int index = Random.Range(0, safetyQuotes.Length);
        displayText.text = safetyQuotes[index];

        // Start scene load timer
        Invoke(nameof(LoadNextScene), 5f);
    }

    private void LoadNextScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Scene to load is not set!");
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
