using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    SaveManager _saveManager;
    SceneLoader _seneLoader;
    GameManagerUI _managerUI;

    public ScoreDataObject scoresData;

    public int levelDifference = 2;
    GameData _gameData;
    List<LevelData> _levelData;
    SettingsData _settingsData;

    Vector2 checkPoint;
    bool startCounting = false;
    float timer = 0;
    int currentMinutes = 0;
    int currentSeconds = 0;
    int currentMoney = 0;
    int currentScore = 0;
    int starA = 0;
    int starB = 0;
    int starC = 0;

    bool internetConnected = false;
    bool showLevels = false;

    bool[] collectablesItems;
    bool firstRun = false;

    public Canvas myCanvas;

    bool checkingNet = false;

    int currentSceneId = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        _saveManager = GetComponent<SaveManager>();
        _seneLoader = GetComponent<SceneLoader>();
        _managerUI = GetComponent<GameManagerUI>();
    }

    private void Start()
    {
        _gameData = _saveManager.loadGameData();
        _levelData = _saveManager.loadLevelsData();
        _settingsData = _saveManager.loadSettingsData();

        collectablesItems = new bool[0];
        firstRun = true;
    }

    private void Update()
    {
        if (startCounting)
        {
            timer += Time.deltaTime;

            currentMinutes = (int)Mathf.Floor(timer / 60f);
            currentSeconds = (int)(timer % 60);

            _managerUI.setTime(currentMinutes + ":" + currentSeconds);
        }

        if (checkingNet)
        {
            checkAmounts();
        }
    }

    public GameData loadGameData()
    {
        return _gameData;
    }

    public LevelData loadLevelData(int id)
    {
        if (id > _levelData.Count - 1)
        {
            LevelData d = new LevelData();
            return d;
        }
        else
        {
            return _levelData[id];
        }
    }

    public SettingsData loadSettingsData()
    {
        return _settingsData;
    }

    public void saveGameData(GameData g)
    {
        _saveManager.saveGameData(g);
        _gameData = _saveManager.loadGameData();
    }
    public void saveLevelData(int id, LevelData d)

    {
        _saveManager.saveLevelData(id, d);
        _levelData = _saveManager.loadLevelsData();
    }

    public void saveSettingsData(SettingsData s)
    {
        _saveManager.saveSettingsData(s);
        _settingsData = _saveManager.loadSettingsData();
    }

    public void loadScene(int id)
    {
        if(id == -1)
        {
            _managerUI.enablePanel(0);
            reloadScene(false);
        }
        else
        {
            _managerUI.enablePanel(0);
            _seneLoader.loadScene(id);
        }

        currentSceneId = id;
        startNewLevel();

    }

    public void loadingStatus(int id)
    {
        if(id == -7)
        {
            resetCheckPoint();
        }

        if (id > 1)
        {
            _managerUI.enablePanel(1);
        }
        else
        {
            if (!firstRun)
            {
                firstRun = true;
            }
            else
            {
                if (showLevels)
                {
                    showPanel(7);
                }
                else
                {
                    showPanel(99);
                }
            }
        }

        showLevels = false;
        myCanvas.worldCamera = Camera.main;
    }

    public void setCheckPoint(Vector2 pos)
    {
        checkPoint = pos;
    }

    public Vector2 getCheckPoint()
    {
        return checkPoint;
    }

    public void setDead(bool b)
    {
        startCounting = false;

        checkAmounts();
        _managerUI.setDead(b);
    }

    public void setFinished(bool b)
    {
        startCounting = false;
        _gameData = _saveManager.loadGameData();
        _gameData.money = currentMoney;

        _saveManager.saveGameData(_gameData);

        if (b)
        {
            checkScore(_seneLoader.getCurrentScene() - levelDifference);
            _managerUI.setFinished(currentMinutes, currentSeconds, currentMoney, currentScore, starA, starB, starC);
        }
    }

    public void retryCheckPoint(int id)
    {
        switch (id)
        {
            case 0:
                _gameData.diamond -= 1;
                saveGameData(_gameData);
                reloadScene(true);
                break;
            case 1:
                AdsManager.Instance.showRewardedAd();
                break;
            case 2:
                _gameData.diamond -= 10;
                saveGameData(_gameData);
                setFinished(true);
                break;
        }
    }

    void reloadScene(bool r)
    {
        _managerUI.enablePanel(0);
        _seneLoader.reloadScene(r);
    }

    void checkAmounts()
    {
        StartCoroutine(checkInternetConnection("https://www.google.com/"));
    }

    IEnumerator checkInternetConnection(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                internetConnected = false;
            }
            else
            {
                internetConnected = true;
            }


            bool repDiamond = false;
            bool skipDiamond = false;
            bool repVideo = false;

            if (_gameData.diamond > 0)
            {
                repDiamond = true;

                if (_gameData.diamond > 10)
                {
                    skipDiamond = true;
                }
                else
                {
                    skipDiamond = false;
                }
            }
            else
            {
                repDiamond = false;
                skipDiamond = false;
            }

            if (internetConnected)
            {
                repVideo = true;
            }
            else
            {
                repVideo = false;
            }

            _managerUI.setButtonsStatus(repDiamond, skipDiamond, repVideo);

            checkingNet = false;
        }
    }

    public void receiveRewardedAd()
    {
        reloadScene(true);
    }

    public void setShowLevels(bool b)
    {
        showLevels = b;
    }

    public bool[] getCollectables()
    {
        return collectablesItems;
    }

    public void setCollectables(bool[] b)
    {
        collectablesItems = new bool[b.Length];
        collectablesItems = b;
    }

    public void addCoin(int amount, int index)
    {
        currentMoney += amount;

        _managerUI.setMoney(currentMoney);
        collectablesItems[index] = false;
    }

    public int getNumOfLevels()
    {
        int n = _saveManager.getNumOfLevels();

        return n;
    }

    public int getLastLevels()
    {
        return _levelData.Count;
    }

    public void showPanel(int id)
    {
        switch (id)
        {
            case 2:
                Time.timeScale = 0;
                break;

            case 7:
                _managerUI.showPanel(7);
                Time.timeScale = 1;
                UIManager.Instance.showPanel(7); ;
                break;

                case 99:
                _managerUI.showPanel(7);
                Time.timeScale = 1;
                break;

            default:
                _managerUI.showPanel(id);
                break;
        }
    }

    void startNewLevel()
    {
        currentMoney = 0;
        timer = 0;
        currentMinutes = 0;
        currentSeconds = 0;
        currentScore = 0;
        _managerUI.setDataBar(currentMoney, _gameData.diamond);
        collectablesItems = new bool[0];

        Time.timeScale = 1;

        if (currentSceneId != 1)
        {
            startCounting = true;
        }
    }

    void resetCheckPoint()
    {
        _managerUI.enablePanel(1);
        Time.timeScale = 1;
        startCounting = true;
    }

    void checkScore(int id)
    {
        currentScore = Mathf.CeilToInt(40000 / (currentMinutes + 1));
        int collected = 0;

        for(int i = 0; i < collectablesItems.Length; i++)
        {
            if (!collectablesItems[i])
            {
                collected += 25;
            }
        }

        currentScore += collected;

        starA = scoresData._scoreData[id].starAValue;
        starB = scoresData._scoreData[id].starBValue;
        starC = scoresData._scoreData[id].starCValue;

        if(_levelData.Count > id)
        {
            if(_levelData[id].score < currentScore)
            {
                LevelData ld = new LevelData();
                int stars = 0;
                
                if(currentScore > starC)
                {
                    stars = 3;
                }
                else
                {
                    if(currentScore > starB)
                    {
                        stars = 2;
                    }
                    else
                    {
                        if(currentScore > starA)
                        {
                            stars = 1;
                        }
                    }
                }

                ld.score = currentScore;
                ld.star = stars;
                ld.minutes = currentMinutes;
                ld.seconds = currentSeconds;

                saveLevelData(id, ld);
            }
        }
        else
        {
            LevelData ld = new LevelData();
            int stars = 0;

            if (currentScore > starC)
            {
                stars = 3;
            }
            else
            {
                if (currentScore > starB)
                {
                    stars = 2;
                }
                else
                {
                    if (currentScore > starA)
                    {
                        stars = 1;
                    }
                }
            }

            ld.score = currentScore;
            ld.star = stars;
            ld.minutes = currentMinutes;
            ld.seconds = currentSeconds;

            saveLevelData(id, ld);

        }
    }
}
