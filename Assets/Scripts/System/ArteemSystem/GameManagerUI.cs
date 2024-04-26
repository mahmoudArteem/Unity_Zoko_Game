using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManagerUI : MonoBehaviour
{
    public GameObject loadingPanel;
    public GameObject inputPanel;
    public GameObject blurPanel;
    public GameObject pausePanel;
    public GameObject deathPanel;
    public GameObject scorePanel;
    public GameObject startAgainPanel;
    public GameObject startMenuPanel;

    int panelId = 0;
    bool showP = false;
    bool panelEnabled = false;
    bool changeTime = false;
    bool finishedAnim = false;
    Transform currentPanel;

    int scoreSum = 0;
    public GameObject finButtonsHolder;
    public TextMeshProUGUI finScoreTxt;
    public TextMeshProUGUI finTimeTxt;
    public TextMeshProUGUI finMoneyTxt;
    public Image[] stars;
    public Sprite starGlow;
    public Sprite starUnlit;

    int currentScore = 0;
    public ParticleSystem[] starsParticle;

    public TextMeshProUGUI moneyTxt;
    public TextMeshProUGUI diamondsTxt;
    public TextMeshProUGUI timeTxt;

    int starA = 0;
    int starB = 0;
    int starC = 0;

    public Button repeatDiamondBtn;
    public Button skipDiamondBtn;
    public Button repeatVideoBtn;
    private void Update()
    {
        if (showP)
        {
            if (!panelEnabled)
            {
                enablePanel(panelId);
            }
            if (changeTime)
            {
                currentPanel.DOPunchScale(new Vector3(0.01f, 0.01f, 0.01f), 0.5f, 0).SetUpdate(true).OnComplete(endMove);
            }
            else
            {
                compEnd();
            }
        }

        if (finishedAnim)
        {
            finishMove();
        }

    }
    void compEnd()
    {
        changeTime = false;
        DOTween.KillAll();
        currentPanel.localScale = new Vector3(1, 1, 1);

        if (panelId == 4)
        {
            finishedAnim = true;
        }

        showP = false;
    }

    void endMove()
    {
        changeTime = false;
    }
    public void showPanel(int id)
    {
        //panelId = id;

        panelEnabled = false;
        if (id == 7)
        {
            Debug.Log("called show panel 7");
            disablePanels();
            DOTween.KillAll();
            //enablePanel(1);
            //Time.timeScale = 1;
        }
        else
        {
            panelId = id;
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

    public void enablePanel(int id)
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
                currentPanel = startAgainPanel.transform;
                blurPanel.SetActive(true);
                startAgainPanel.SetActive(true);
                break;
            case 6:
                currentPanel = startMenuPanel.transform;
                blurPanel.SetActive(true);
                startMenuPanel.SetActive(true);
                break;
        }

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

    public void setFinished(int min, int sec, int mon, int sco, int st1, int st2, int st3)
    {
        scoreSum = 0;
        currentScore = sco;
        starA = st1;
        starB = st2;
        starC = st3;

        finButtonsHolder.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            stars[i].sprite = starUnlit;
        }

        StartCoroutine(timedFinishPanel(min, sec, mon));
    }

    IEnumerator timedFinishPanel(int min, int sec, int mon)
    {
        finTimeTxt.text = min + ":" + sec;
        finMoneyTxt.text = mon.ToString();

        yield return new WaitForSeconds(3);
        showPanel(4);
    }

    void finishMove()
    {
        if (scoreSum < currentScore)
        {
            scoreSum += 100;
        }
        else
        {
            if (scoreSum > currentScore)
            {
                scoreSum = currentScore;
            }
        }

        finScoreTxt.text = scoreSum.ToString();

        if (scoreSum == starA)
        {
            stars[0].sprite = starGlow;
            if (!starsParticle[0].isPlaying)
            {
                starsParticle[0].Play();
            }
        }

        if (scoreSum == starB)
        {
            stars[1].sprite = starGlow;
            if (!starsParticle[1].isPlaying)
            {
                starsParticle[1].Play();
            }
        }

        if (scoreSum == starC)
        {
            stars[2].sprite = starGlow;
            if (!starsParticle[2].isPlaying)
            {
                starsParticle[2].Play();
            }
        }

        if (scoreSum == currentScore)
        {
            finButtonsHolder.SetActive(true);
            finishedAnim = false;
        }
    }

    public void setTime(string s)
    {
        timeTxt.text = s;

    }

    public void setMoney(int mon)
    {
        moneyTxt.text = mon.ToString();
    }

    public void setButtonsStatus(bool repDiamond, bool skipDiamond, bool repVideo)
    {
        repeatDiamondBtn.interactable = repDiamond;
        skipDiamondBtn.interactable = skipDiamond;
        repeatVideoBtn.interactable = repVideo;

    }

    public void setDataBar(int mon, int diam)
    {
        moneyTxt.text = mon.ToString();
        timeTxt.text = "00:00";
        diamondsTxt.text = diam.ToString();
    }
}
