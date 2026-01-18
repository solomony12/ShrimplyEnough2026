using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdentificationSystem : MonoBehaviour
{
    public static IdentificationSystem Instance { get; private set; }

    private PlayerController playerController;

    private const string identificationSceneName = "IdentificationSystemDisplay";

    [Header("List Items")]
    private TMP_Text baseScannedItem;
    private TMP_Text basePossibleItem;
    private List<TMP_Text> scannedItemClones;
    private List<TMP_Text> possibleItemClones;

    private EvidenceList evidenceList;

    [Header("OSHRIMP Data for Scan")]
    private Evidence currentEvidence;

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

        playerController = PlayerController.Instance;

        scannedItemClones = new List<TMP_Text>();
        possibleItemClones = new List<TMP_Text>();

        ReadJsonForEvidence();
    }

    private void ReadJsonForEvidence()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("evidence_list");
        string json = jsonFile.text;

        evidenceList = JsonUtility.FromJson<EvidenceList>(json);
    }

    public IEnumerator StartDisplaySystem(string evidenceName)
    {
        playerController.DisablePlayerControl();

        yield return SceneManager.LoadSceneAsync(identificationSceneName, LoadSceneMode.Additive);

        FindBaseItems();

        LoadEvidenceScan(evidenceName);
    }

    private void FindBaseItems()
    {
        try
        {
            baseScannedItem = GameObject.FindWithTag("ScannedText").GetComponent<TMP_Text>();
            basePossibleItem = GameObject.FindWithTag("PossibleText").GetComponent<TMP_Text>();
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
        currentEvidence = GetEvidence(evidenceName);
        if (currentEvidence == null)
        {
            Debug.LogError("Evidence not found: " + evidenceName);
            return;
        }

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
            clone.transform.localPosition -= new Vector3(0, baseScannedItem.GetPreferredValues().y * scannedIndex, 0);

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
            clone.transform.localPosition -= new Vector3(0, basePossibleItem.GetPreferredValues().y * possibleIndex, 0);

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
        foreach (var item in scannedItemClones) Destroy(item.gameObject);
        scannedItemClones.Clear();

        foreach (var item in possibleItemClones) Destroy(item.gameObject);
        possibleItemClones.Clear();
    }

    Evidence GetEvidence(string name)
    {
        foreach (Evidence e in evidenceList.evidences)
        {
            if (e.evidenceName == name)
                return e;
        }
        return null;
    }
}

[System.Serializable]
public class Evidence
{
    public string evidenceName;
    public string[] scannedCompounds;
    public string[] possibleItems;
    public string correctAnswer;
}

[System.Serializable]
public class EvidenceList
{
    public Evidence[] evidences;
}
