using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IntroVideo : MonoBehaviour
{
    public bool started = false;

    void Update()
    {
        if (started == false)
        {
            started = true;
            StartCoroutine(PlayMovie());
        }
    }

    public IEnumerator PlayMovie()
    {  
        Handheld.PlayFullScreenMovie("ArteemLogo.mp4",
        Color.black, FullScreenMovieControlMode.Hidden,
        FullScreenMovieScalingMode.AspectFit);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(1);
    }
}
