using System.Collections.Generic;
using UnityEngine;

public class DictionaryExample : MonoBehaviour
{
    public List<StringIntPair> pairsList = new List<StringIntPair>();

    private Dictionary<string, int> dictionary = new Dictionary<string, int>();

    void Start()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        dictionary.Clear();
        foreach (var pair in pairsList)
        {
            if (!dictionary.ContainsKey(pair.key))
            {
                dictionary.Add(pair.key, pair.value);
            }
        }

        // Example usage
        if (dictionary.ContainsKey("exampleKey"))
        {
            Debug.LogError("Value for 'exampleKey': " + dictionary["exampleKey"]);
        }
    }
}

[System.Serializable]
public class StringIntPair
{
    public string key;
    public int value;
}
