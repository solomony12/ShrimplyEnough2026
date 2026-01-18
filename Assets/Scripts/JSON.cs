using System.IO;
//using Unity.Plastic.Newtonsoft.Json;
//using System.Text;
using UnityEngine;
public class EvidenceWrapper
{
    public Evidence[] evidences; // A field name that matches a key in your wrapped JSON
}
public class JSON : MonoBehaviour
{
    [SerializeField] TextAsset jsonFile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string jsonString = jsonFile.text;
        // Evidence evidence= new Evidence();
        //Evidence evidence = JsonSerializer.Deserialize<Evidence>(jsonString);
        //evidence.
        // need to wrap this shit bc uhhhh... no native support array uhhhh... I don't fucking know.
        string wrappedJson = "{\"evidence\":" + jsonString + "}";

        EvidenceWrapper wrapper = JsonUtility.FromJson<EvidenceWrapper>(wrappedJson);
        foreach (var evidece in wrapper.evidences)
        {
            Debug.Log(evidece.evidenceName);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
