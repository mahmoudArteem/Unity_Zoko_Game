using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    int sceneId;
    public Slider loadingBar;
    bool startLoading = false;
    bool calledStartCourt = false;
    bool reloadCheckPoint = false;

    private void Update()
    {
        if (startLoading)
        {
            loadingBar.value = Mathf.Lerp(loadingBar.value, 0.7f, 5 * Time.deltaTime);

            if (!calledStartCourt)
            {
                StartCoroutine(loadNewScene());

                calledStartCourt = true;
            }

        }
    }
    public void loadScene(int id)
    {
        sceneId = id;
        loadingBar.value = 0;
        calledStartCourt = false;
        startLoading = true;
    }

    IEnumerator loadNewScene()
    {
        yield return new WaitForSeconds(1);

        startLoading = false;

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneId);

        while (!async.isDone)
        {
            loadingBar.value += Time.deltaTime;
            yield return null;
        }

        if (!reloadCheckPoint)
        {
            GameManager.Instance.loadingStatus(sceneId);
        }
        else
        {
            GameManager.Instance.loadingStatus(-7);
        }

        reloadCheckPoint = false;
    }

    public void reloadScene(bool r)
    {
        sceneId = SceneManager.GetActiveScene().buildIndex;
        reloadCheckPoint = r;
        loadScene(sceneId);
    }

    public int getCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
