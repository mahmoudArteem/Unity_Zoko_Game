using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.IO;
public class gm2 : MonoBehaviour
{
    public static gm2 Instance { get; private set; }
    GameData _gameData;
    public GameObject loadingPanel;
    public GameObject inputPanel;
    public GameObject blurPanel;
    public GameObject pausePanel;
    public GameObject deathPanel;
    public GameObject scorePanel;
    public GameObject startAgainPanel;
    public GameObject startMenuPanel;

    public RectTransform panelRect;
    public RectTransform sureRect;
    public Slider loadingBar;
    public Canvas myCanvas;

    bool showP = false;
    bool startLoading = false;
    int currentPanelId = 0;
    int currentSceneId = 0;
    Transform currentPanel;

    Vector2 checkPoint;
    bool changeTime = false;
    bool firstRun = false;
    bool calledCort = false;
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
    }

    private void Start()
    {
        //_gameData = SaveManager.Instance.loadGameData();
        arteemTest();
    }

    private void Update()
    {
        if (showP)
        {
            enablePanel(currentPanelId);
            if (changeTime)
            {
                currentPanel.DOPunchScale(new Vector3(0.01f, 0.01f, 0.01f), 0.5f, 0).SetUpdate(true).OnComplete(endMove);
            }
            else
            {
                compEnd();
            }
        }

        if (startLoading)
        {
            loadingBar.value = Mathf.Lerp(loadingBar.value, 0.7f, 5 * Time.deltaTime);

            if (!calledCort)
            {
                StartCoroutine(loadNewScene());
                calledCort = true;
            }
        }
    }
    public void showPanel(int id)
    {
        if (id == 7)
        {
            disablePanels();
            DOTween.KillAll();
            Time.timeScale = 1;
        }
        else
        {
            currentPanelId = id;
            if (id > 1)
            {
                changeTime = true;
                showP = true;
            }
            else
            {
                if (id == -1)
                {
                    currentPanel.gameObject.SetActive(false);
                }
                else
                {
                    disablePanels();
                    enablePanel(id);
                }
            }
        }
    }
    void disablePanels()
    {
        loadingPanel.SetActive(false);
        inputPanel.SetActive(false);
        blurPanel.SetActive(false);
        pausePanel.SetActive(false);
        deathPanel.SetActive(false);
        scorePanel.SetActive(false);
        startAgainPanel.SetActive(false);
        startMenuPanel.SetActive(false);
    }

    void enablePanel(int id)
    {
        switch (id)
        {
            case 0:
                disablePanels();
                loadingPanel.SetActive(true);
                break;
            case 1:
                disablePanels();
                inputPanel.SetActive(true);
                break;
            case 2:
                disablePanels();
                currentPanel = pausePanel.transform;
                Time.timeScale = 0;
                blurPanel.SetActive(true);
                pausePanel.SetActive(true);
                break;
            case 3:
                disablePanels();
                currentPanel = deathPanel.transform;
                blurPanel.SetActive(true);
                deathPanel.SetActive(true);
                break;
            case 4:
                disablePanels();
                currentPanel = scorePanel.transform;
                blurPanel.SetActive(true);
                scorePanel.SetActive(true);
                break;
            case 5:
                //disablePanels();
                currentPanel = startAgainPanel.transform;
                blurPanel.SetActive(true);
                startAgainPanel.SetActive(true);
                break;
            case 6:
                //disablePanels();
                currentPanel = startMenuPanel.transform;
                blurPanel.SetActive(true);
                startMenuPanel.SetActive(true);
                break;
        }
    }


    void endMove()
    {
        changeTime = false;
    }

    void compEnd()
    {
        changeTime = false;
        DOTween.KillAll();
        currentPanel.localScale = new Vector3(1, 1, 1);
        showP = false;
    }

    public void setCheckPoint(Vector2 pos)
    {
        checkPoint = pos;
    }

    public Vector2 getCheckPoint()
    {
        return checkPoint;
    }

    public void setFinished(bool b)
    {
        StartCoroutine(timedFinishPanel());
    }

    public void setDead(bool b)
    {
        StartCoroutine(timedDeadPanel());
    }

    IEnumerator timedDeadPanel()
    {
        yield return new WaitForSeconds(3);
        showPanel(3);
    }

    IEnumerator timedFinishPanel()
    {
        yield return new WaitForSeconds(3);
        showPanel(4);
    }

    public void loadScene(int id)
    {
        resetAll();
        loadingBar.value = 0;
        if(id == -1)
        {
            currentSceneId = SceneManager.GetActiveScene().buildIndex;
        }
        else
        {
            currentSceneId = id;
        }
        
        startLoading = true;
        enablePanel(0);
    }

    IEnumerator loadNewScene()
    {
        yield return new WaitForSeconds(1);

        startLoading = false;
        AsyncOperation async = SceneManager.LoadSceneAsync(currentSceneId);

        while (!async.isDone)
        {
            loadingBar.value += Time.deltaTime;
            yield return null;
        }

        resetAll();

        if (currentSceneId > 1)
        {
            enablePanel(1);
        }
        else
        {
            if (firstRun)
            {
                UIManager.Instance.showPanel(1);
            }
            else
            {
                firstRun = true;
            }
        }

        myCanvas.worldCamera = Camera.main;
    }

    void resetAll()
    {
        disablePanels();
        DOTween.KillAll();
        showP = false;
        changeTime = false;
        Time.timeScale = 1;
        checkPoint = new Vector2(0, 0);
        calledCort = false;
    }

    void arteemTest()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "/Levels/");
        FileInfo[] info = dir.GetFiles("*.unity");
        Debug.Log("num of levels = " + info.Length);
    }

}

