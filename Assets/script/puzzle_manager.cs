using System.Collections.Generic;
using UnityEngine;

public class MemoryPuzzleManager : MonoBehaviour
{
    public static MemoryPuzzleManager Instance;

    [System.Serializable]
    public class MemoryEntry
    {
        public string objectName;
        public string keyword;
        public string truthText;

        public MemoryEntry(string obj, string key, string truth)
        {
            objectName = obj;
            keyword = key;
            truthText = truth;
        }
    }

    public List<MemoryEntry> revealedTruths = new List<MemoryEntry>();

    [HideInInspector] public bool libroFound = false;
    [HideInInspector] public bool cartasFound = false;
    [HideInInspector] public bool crucifijoFound = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterTruth(string objectName, string keyword, string truth, InteractionObject obj)
    {
        foreach (var entry in revealedTruths)
        {
            if (entry.objectName == objectName)
                return;
        }

        revealedTruths.Add(new MemoryEntry(objectName, keyword, truth));
        Debug.Log($"[MemoryPuzzle] VERDAD REVELADA → {objectName} | KEY: {keyword}");
    }

    public bool AllObjectsInteracted()
    {
        return libroFound && cartasFound && crucifijoFound;
    }

    public void RegisterObject(string name)
    {
        if (name.ToLower().Contains("libro")) libroFound = true;
        if (name.ToLower().Contains("cartas")) cartasFound = true;
        if (name.ToLower().Contains("crucifijo")) crucifijoFound = true;
    }
}
