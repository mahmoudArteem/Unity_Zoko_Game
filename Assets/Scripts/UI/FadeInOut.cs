using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeInOut : MonoBehaviour
{
    TextMeshProUGUI txt;
    Color color;
    private void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
        color = txt.color;
    }

    private void Update()
    {
        txt.color = new Color(color.r, color.g, color.b, Mathf.Sin(Time.time * 5) * 0.5f + 0.5f);
    }
}
