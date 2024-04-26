using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{

    public string key;

    // Use this for initialization
    void Start()
    {
        //updateText();
    }

    public void updateText()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.font = LocalizationManager.instance.getFont();
        //LocalizationManager.instance.LoadLocalizedText("arabicM.json");
        text.text = LocalizationManager.instance.GetLocalizedValue(key.ToString());
    }

}
