using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using ArabicSupport;

public class LocalizationManager : MonoBehaviour
{

    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";
    public TMP_FontAsset arabicFont;
    public TMP_FontAsset englishFont;
    public bool arabic = false;
    public LocalizedText[] translatedItems;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLocalizedText(string fileName)
    {
        if(fileName == "arabicM.json")
        {
            arabic = true;
        }
        else
        {
            arabic = false;
        }
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        isReady = true;
        updateLanguage();
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];

            if (arabic)
            {
                result = ArabicFixer.Fix(result, true);
            }
        }

        return result;

    }

    public bool GetIsReady()
    {
        return isReady;
    }

    public TMP_FontAsset getFont()
    {
        if (arabic)
        {
            return arabicFont;
        }
        else
        {
            return englishFont;
        }
    }

    public void updateLanguage()
    {
        for(int i = 0; i < translatedItems.Length; i++)
        {
            if(translatedItems[i] != null)
            translatedItems[i].updateText();
        }
    }

}
