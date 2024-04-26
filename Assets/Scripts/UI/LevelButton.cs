using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    int myId = 0;
    public Button bt;
    public GameObject lockSprite;
    public TextMeshProUGUI lvlName;
    public Sprite starGlow;
    public Sprite starUnlit;
    public Image[] stars;


    public void setLevelId(int id, bool locked, int star)
    {
        myId = id;
        lvlName.text = (id - 1) + "";

        if (locked)
        {
            bt.interactable = false;
        }
        else
        {
            lockSprite.SetActive(false);
        }

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

    }
    public void clickMe()
    {
        UIManager.Instance.setCurrentLevel(myId);
    }
}
