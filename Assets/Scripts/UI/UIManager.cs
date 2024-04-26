using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Audio;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    bool showP = false;
    bool showSubP = false;
    bool showPop = false;
    Vector2 posUp;
    Vector2 posDown;
    Vector2 nextPoint;

    int currentPanelId = 0;
    int subPanelId = 0;

    public RectTransform parentRect;
    public RectTransform blurRect;
    public GameObject topBar;
    public GameObject navBar;
    public GameObject blurPanel;
    public GameObject logoPanel;
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject customizePanel;
    public GameObject storePanel;
    public GameObject languagePanel;
    public GameObject aboutPanel;
    public GameObject levelsPanel;
    public GameObject levelsInfoPanel;

    public ScrollRect hatsHolder;
    public ScrollRect customsHolder;

    public Transform storeHolder;
    public RectTransform storeParent;
    public Transform customizeHolder;
    public RectTransform customizeParent;

    public GameObject levelsHolder;
    public GameObject levelPref;

    public Button[] mainButtons;
    public Button[] subButtons;

    public Button saveBtn;
    public Slider musicSlider;
    public Slider sfxSlider;

    public GameObject zokoPrefUI;

    float storeWidth;
    float customizeWidth;

    Transform currentPanel;

    //levelInfo things 

    int levelId = 0;
    public TextMeshProUGUI levelName;
    public Sprite starGlow;
    public Sprite starUnlit;
    public Image[] stars;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI timeTxt;

    public TextMeshProUGUI navMoneyTxt;
    public TextMeshProUGUI navCashTxt;
    public TextMeshProUGUI navDiamondsTxt;

    public AudioMixer _audioMixer;


    SettingsData _settingsData;
    GameData _gameData;
    LevelData levelData;

    float prevMusic = 0;
    float prevSfx = 0;
    bool panelEnabled = false;

    int numOfLevels = 0;
    int levelDifference = 0;
    int lastLevel = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {

        float heightBlur = blurRect.rect.height;
        storeWidth = storeParent.rect.width;
        customizeWidth = customizeParent.rect.width;
        blurRect.transform.localPosition = new Vector2(0, -(heightBlur / 2));

        blurRect.anchorMin = new Vector2(0.5f, 1);
        blurRect.anchorMax = new Vector2(0.5f, 1);
        blurRect.pivot = new Vector2(0.5f, 0.5f);
        blurRect.sizeDelta = parentRect.rect.size;

        posUp = new Vector2(0, heightBlur);
        posDown = new Vector2(0, -(heightBlur / 2));

        nextPoint = posUp;

        disablePanels();
        topBar.SetActive(false);
        navBar.SetActive(false);
        logoPanel.SetActive(true);

        updateGameData();

        createLevels();
        // LocalizationManager.instance.LoadLocalizedText("arabicM.json");
    }

    private void Update()
    {
        if (showP)
        {
            blurPanel.transform.DOLocalMoveY(nextPoint.y, 0.5f).OnComplete(moveDown);
        }

        if (showSubP)
        {
            if (!showP)
            {
                moveSubPanel();
            }
        }

        if (showPop)
        {
            currentPanel.DOPunchScale(new Vector3(0.01f, 0.01f, 0.01f), 0.5f, 0).SetUpdate(true).OnComplete(endPopMove);
        }
    }

    void moveDown()
    {
        nextPoint = posDown;

        if (!panelEnabled)
        {
            enablePanel(currentPanelId);
        }

        blurPanel.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBounce).OnComplete(endMove);
        showP = false;
    }

    void endMove()
    {
        nextPoint = posUp;
        showP = false;
        DOTween.KillAll();
    }

    void moveSubPanel()
    {
        switch (subPanelId)
        {
            case 0:
                storeHolder.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutBounce).OnComplete(subEndMove);
                break;
            case 1:
                storeHolder.DOLocalMoveX(-storeWidth, 0.5f).SetEase(Ease.OutBounce).OnComplete(subEndMove);
                break;
            case 2:
                storeHolder.DOLocalMoveX(-storeWidth * 2, 0.5f).SetEase(Ease.OutBounce).OnComplete(subEndMove);
                break;
            case 3:
                customizeHolder.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutBounce).OnComplete(subEndMove);
                hatsHolder.verticalNormalizedPosition = 1;
                break;
            case 4:
                customizeHolder.DOLocalMoveX(-customizeWidth, 0.5f).SetEase(Ease.OutBounce).OnComplete(subEndMove);
                customsHolder.verticalNormalizedPosition = 1;
                break;
        }
    }
    void subEndMove()
    {
        showSubP = false;
        DOTween.KillAll();
    }

    void endPopMove()
    {
        showPop = false;
        DOTween.KillAll();
    }

    public void showPanel(int id)
    {
        nextPoint = posUp;
        currentPanelId = id;
        if (zokoPrefUI != null)
        {
            zokoPrefUI.SetActive(false);
        }
        panelEnabled = false;
        showP = true;
    }

    public void showPopPanel(int id)
    {
        //currentPanelId = id;
        switch (id)
        {
            case 8:
                currentPanel = levelsInfoPanel.transform;
                levelsInfoPanel.SetActive(true);
                break;
            case 99:
                levelsInfoPanel.SetActive(false);
                break;
        }

        showPop = true;
    }

    void disablePanels()
    {
        logoPanel.SetActive(false);
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        customizePanel.SetActive(false);
        storePanel.SetActive(false);
        languagePanel.SetActive(false);
        levelsPanel.SetActive(false);
        levelsInfoPanel.SetActive(false);
        aboutPanel.SetActive(false);

        topBar.SetActive(true);
        navBar.SetActive(true);
    }

    void enablePanel(int id)
    {
        switch (id)
        {
            case 0:
                disablePanels();
                logoPanel.SetActive(true);
                break;
            case 1:
                disablePanels();
                mainPanel.SetActive(true);
                break;
            case 2:
                disablePanels();
                loadSettings();
                saveBtn.interactable = false;
                settingsPanel.SetActive(true);
                break;
            case 3:
                disablePanels();
                customizePanel.SetActive(true);
                StartCoroutine(enableZoko());
                break;
            case 4:
                disablePanels();
                storePanel.SetActive(true);
                break;
            case 5:
                disablePanels();
                languagePanel.SetActive(true);
                break;
            case 6:
                disablePanels();
                aboutPanel.SetActive(true);
                break;
            case 7:
                disablePanels();
                levelsPanel.SetActive(true);
                break;
            case 8:
                disablePanels();
                levelsInfoPanel.SetActive(true);
                break;
        }

        panelEnabled = true;
    }

    public void disableButtons(int id)
    {
        for (int i = 0; i < mainButtons.Length; i++)
        {
            if (i == id)
            {
                mainButtons[i].interactable = false;
            }
            else
            {
                mainButtons[i].interactable = true;
            }
        }
    }

    public void disableSubButtons(int id)
    {
        for (int i = 0; i < subButtons.Length; i++)
        {
            if (i == id)
            {
                subButtons[i].interactable = false;
            }
            else
            {
                subButtons[i].interactable = true;
            }
        }
    }

    public void showSubPanel(int id)
    {
        subPanelId = id;
        showSubP = true;
    }

    IEnumerator enableZoko()
    {
        yield return new WaitForSeconds(0.7f);
        zokoPrefUI.SetActive(true);
    }

    public void createLevels()
    {
        numOfLevels = GameManager.Instance.getNumOfLevels();
        levelDifference = GameManager.Instance.levelDifference;
        lastLevel = GameManager.Instance.getLastLevels();

        for (int i = 0; i < numOfLevels; i++)
        {
            LevelData d = GameManager.Instance.loadLevelData(i);
            GameObject lev = Instantiate(levelPref, levelsHolder.transform, false);

            if (i > lastLevel)
            {
                lev.GetComponent<LevelButton>().setLevelId(i + levelDifference, true, 0);
            }
            else
            {
                int star = d.star;
                lev.GetComponent<LevelButton>().setLevelId(i + levelDifference, false, star);
            }
            //lev.GetComponent<LevelButton>().setLevelId(i + 1);
        }

        updateGameData();
    }

    public void setCurrentLevel(int id)
    {
        updateGameData();

        LevelData d = GameManager.Instance.loadLevelData(id - levelDifference);
        int star = d.star;
        int score = d.score;
        string t = d.minutes + ":" + d.seconds;
        levelId = id;
        levelName.text = "LEVEL " + (id - 1);

        switch (star)
        {
            case 3:
                stars[0].sprite = starGlow;
                stars[1].sprite = starGlow;
                stars[2].sprite = starGlow;
                break;
            case 2:
                stars[0].sprite = starGlow;
                stars[1].sprite = starGlow;
                break;
            case 1:
                stars[0].sprite = starGlow;
                break;
            case 0:
                stars[0].sprite = starUnlit;
                stars[1].sprite = starUnlit;
                stars[2].sprite = starUnlit;
                break;
        }

        scoreTxt.text = score.ToString();
        timeTxt.text = t;
        showPopPanel(8);
    }

    public void runLevel()
    {
        GameManager.Instance.loadScene(levelId);
    }

    void loadSettings()
    {
        _settingsData = GameManager.Instance.loadSettingsData();

        musicSlider.value = _settingsData.musicValue;
        sfxSlider.value = _settingsData.sfxValue;
    }

    public void saveSettings()
    {
        SettingsData sd = new SettingsData();

        sd.musicValue = musicSlider.value;
        sd.sfxValue = sfxSlider.value;

        GameManager.Instance.saveSettingsData(sd);

        _audioMixer.SetFloat("musicVol", sd.musicValue);
        _audioMixer.SetFloat("sfxVol", sd.sfxValue);

        saveBtn.interactable = false;
    }
    public void checkSaveChanges()
    {
        saveBtn.interactable = true;
    }

    public void defaultBtn()
    {
        musicSlider.value = prevMusic;
        sfxSlider.value = prevSfx;

        saveSettings();
        saveBtn.interactable = false;
    }

    void updateGameData()
    {
        _gameData = GameManager.Instance.loadGameData();

        navMoneyTxt.text = _gameData.money.ToString();
        navCashTxt.text = _gameData.cash.ToString();
        navDiamondsTxt.text = _gameData.diamond.ToString();
    }

}
