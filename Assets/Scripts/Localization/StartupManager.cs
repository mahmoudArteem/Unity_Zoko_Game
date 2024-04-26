using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    private void Start()
    {
        LocalizationManager.instance.LoadLocalizedText("arabicM.json");
        //SceneManager.LoadScene(5);
        //StartCoroutine(mStart());
    }
    // Use this for initialization
   /* private IEnumerator mStart()
    {
        while (!LocalizationManager.instance.GetIsReady())
        {
            Debug.Log("not loaded yet");
            yield return null;
        }

        Debug.Log("should be loaded");
        SceneManager.LoadScene(5);
    }*/

}
