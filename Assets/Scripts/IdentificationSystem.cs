using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IdentificationSystem : MonoBehaviour
{
    public static IdentificationSystem Instance { get; private set; }

    private PlayerInputHandler inputHandler;

    private const string identificationSceneName = "IdentificationSystemDisplay";

    [Header("List Items")]
    private TMP_Text baseScannedItem;
    private TMP_Text basePossibleItem;
    private List<TMP_Text> scannedItemClones;
    private List<TMP_Text> possibleItemClones;
    private GameObject selectorBox;
    private Vector3 selectorStartingPos;
    private Transform selectorPosition;

    private EvidenceList evidenceList;

    [Header("OSHRIMP Data for Scan")]
    private Evidence currentEvidence;
    private int currentEvidenceIndex;
    private int correctIndex;
    private float[] evidencePercentages;
    private float[] normsMultiplier;

    private float itemSize;

    [Header("Selector Box")]
    [SerializeField] private int currentIndex;
    private RectTransform selectorRect;
    private Vector2 selectorStartingAnchoredPos;
    private float inputDelay = 0.15f;
    private float inputTimer = 0f;

    [SerializeField] private AudioClip uiClip;

    public static bool isOisSystemUp;

    private bool thresholdReached = false;

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

        inputHandler = PlayerInputHandler.Instance;

        scannedItemClones = new List<TMP_Text>();
        possibleItemClones = new List<TMP_Text>();

        currentIndex = 0;

        thresholdReached = false;

        uiClip = Resources.Load<AudioClip>("Sounds/ui-button-sound-cancel-back-exit-continue-467877");

        ReadJsonForEvidence();
        CalculateNorms();
    }

    private void Update()
    {
        if (!ScanEvidence.IsDisplayOpen || SettingsMenuUI.SettingsIsOpen)
            return;

        inputTimer -= Time.deltaTime;

        if (inputTimer > 0) return;

        if (inputHandler.ItemUpTriggered || inputHandler.ItemDownTriggered || inputHandler.ItemSelectTriggered)
        {
            HandleItemSelection();
            inputTimer = inputDelay;
        }
    }

    private void ReadJsonForEvidence()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("evidence_list");
        string json = jsonFile.text;

        evidenceList = JsonUtility.FromJson<EvidenceList>(json);
    }

    // Normalization for how much each evidence scan should add
    private void CalculateNorms()
    {
        normsMultiplier = new float[evidenceList.evidences.Length];

        float totalSumPercentage = 0;
        // Get total sum of all correct answers' percentage-wise
        foreach (Evidence evidence in evidenceList.evidences)
        {
            float correctAmount = evidence.rightPercentages[evidence.correctAnswer];
            totalSumPercentage += correctAmount;
        }

        // Normalize each value and store the multiplier (which should be a decimal amount < 1)
        for(int i = 0; i < evidenceList.evidences.Length; i++)
        {
            Evidence evidence = evidenceList.evidences[i];
            normsMultiplier[i] = evidence.rightPercentages[evidence.correctAnswer] / totalSumPercentage;
        }
        Debug.Log("Norms calculated");
    }

    public IEnumerator StartDisplaySystem(string evidenceName)
    {
        PlayerController.DisablePlayerControl();

        isOisSystemUp = true;

        yield return SceneManager.LoadSceneAsync(identificationSceneName, LoadSceneMode.Additive);

        RemoveDuplicateEventSystems();

        FindBaseItems();

        selectorPosition = selectorBox.transform;
        selectorStartingPos = selectorPosition.position;
        currentIndex = 0;
        itemSize = baseScannedItem.GetPreferredValues().y;
        selectorRect = selectorBox.GetComponent<RectTransform>();
        selectorStartingAnchoredPos = selectorRect.anchoredPosition;

        LoadEvidenceScan(evidenceName);

        ScanEvidence.IsDisplayOpen = true;
    }

    private void EndDisplaySystem()
    {
        ScanEvidence.IsDisplayOpen = false;

        PlayerController.EnablePlayerControl();

        if (SceneManager.GetSceneByName(identificationSceneName).isLoaded)
        {
            isOisSystemUp = false;
            SceneManager.UnloadSceneAsync(identificationSceneName);
        }
    }

    private void RemoveDuplicateEventSystems()
    {
        EventSystem[] eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        if (eventSystems.Length > 1)
        {
            // Keep the first one and destroy the rest
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);
            }
        }
    }

    private void FindBaseItems()
    {
        try
        {
            baseScannedItem = GameObject.FindWithTag("ScannedText").GetComponent<TMP_Text>();
            basePossibleItem = GameObject.FindWithTag("PossibleText").GetComponent<TMP_Text>();
            selectorBox = GameObject.FindWithTag("SelectorBox");
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("The base item(s) in the Identifiation System were not found.");
        }
    }

    private void LoadEvidenceScan(string evidenceName)
    {
        ResetScreen();

        // Get new evidence info
        currentEvidence = GetEvidence(evidenceName, out currentEvidenceIndex);
        if (currentEvidence == null)
        {
            Debug.LogError("Evidence not found: " + evidenceName);
            return;
        }

        // Get correct answer
        correctIndex = currentEvidence.correctAnswer;
        // Get percentages
        evidencePercentages = currentEvidence.rightPercentages;

        // Create scanned compounds list
        int scannedIndex = 0;
        foreach (string scannedCompound in currentEvidence.scannedCompounds)
        {
            TMP_Text clone = Instantiate(baseScannedItem, baseScannedItem.transform.parent);
            clone.text = $"- {scannedCompound}";

            // Place it at the same position as base
            clone.transform.localPosition = baseScannedItem.transform.localPosition;
            clone.transform.localRotation = baseScannedItem.transform.localRotation;
            clone.transform.localScale = baseScannedItem.transform.localScale;

            // Move it down based on index
            clone.transform.localPosition -= new Vector3(0, itemSize * scannedIndex, 0);

            scannedItemClones.Add(clone);
            scannedIndex++;
        }

        // Create possible matches list
        int possibleIndex = 0;
        foreach (string possibleMatch in currentEvidence.possibleItems)
        {
            TMP_Text clone = Instantiate(basePossibleItem, basePossibleItem.transform.parent);
            clone.text = $"- {possibleMatch}";

            // Place it at the same position as base
            clone.transform.localPosition = basePossibleItem.transform.localPosition;
            clone.transform.localRotation = basePossibleItem.transform.localRotation;
            clone.transform.localScale = basePossibleItem.transform.localScale;

            // Move it down based on index
            clone.transform.localPosition -= new Vector3(0, itemSize * possibleIndex, 0);

            possibleItemClones.Add(clone);
            possibleIndex++;
        }

        // Hide base items
        baseScannedItem.enabled = false;
        basePossibleItem.enabled = false;
    }

    private void ResetScreen()
    {
        baseScannedItem.enabled = true;
        basePossibleItem.enabled = true;

        // Destroy old items
        if (scannedItemClones != null)
        {
            foreach (var item in scannedItemClones)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            scannedItemClones.Clear();
        }

        if (possibleItemClones != null)
        {
            foreach (var item in possibleItemClones)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            possibleItemClones.Clear();
        }
    }

    Evidence GetEvidence(string name, out int index)
    {
        for (int i = 0; i < evidenceList.evidences.Length; i++)
        {
            if (evidenceList.evidences[i].evidenceName == name)
            {
                index = i;
                return evidenceList.evidences[i];
            }
        }

        index = -1;
        return null;
    }

    void HandleItemSelection()
    {
        AudioManager.Instance.PlaySFX(uiClip);

        int listLength = possibleItemClones.Count;

        if (inputHandler.ItemSelectTriggered)
        {
            // New system adds based on percentage (soft answer rather than hard answer)
            float percentToAdd = evidencePercentages[currentIndex] * normsMultiplier[currentEvidenceIndex];
            Debug.Log(
                $"percentToAdd = {percentToAdd} " +
                $"(evidencePercentages[{currentIndex}] = {evidencePercentages[currentIndex]}, " +
                $"normsMultiplier[{currentEvidenceIndex}] = {normsMultiplier[currentEvidenceIndex]})"
            );
            thresholdReached = EvidenceGeneration.AddPercentage(percentToAdd);

            // Old system only had one correct answer
            /*
            if (currentIndex == correctIndex)
            {
                EvidenceGeneration.CorrectEvidenceAmount++;
                Debug.Log("You guessed correctly!");
            }
            Debug.Log("Evidence Got: " + EvidenceGeneration.CorrectEvidenceAmount.ToString());
            */

            EndDisplaySystem();

            // If threshold passed for correct evidence, give the player the option to return when they want (OSHRIMP ending)
            // TODO:
            if (thresholdReached)
            { }

            return;
        }

        if (inputHandler.ItemUpTriggered)
            currentIndex--;

        if (inputHandler.ItemDownTriggered)
            currentIndex++;

        if (currentIndex < 0)
            currentIndex = listLength - 1;

        if (currentIndex >= listLength)
            currentIndex = 0;

        selectorRect.anchoredPosition = selectorStartingAnchoredPos + new Vector2(0, -itemSize * currentIndex);
    }

}

[System.Serializable]
public class Evidence
{
    public string evidenceName;
    public string[] scannedCompounds;
    public string[] possibleItems;
    public int correctAnswer;
    public float[] rightPercentages;
}

[System.Serializable]
public class EvidenceList
{
    public Evidence[] evidences;
}
