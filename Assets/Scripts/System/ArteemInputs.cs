using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArteemInputs : MonoBehaviour
{
    public static ArteemInputs Instance { get; private set; }

    public VariableJoystick stick;
    public Image background;
    public Sprite[] axisSprites;
    public float horizontal = 0f;
    public float vertical = 0f;
    public bool jumpInput = false;
    public bool slideInput = false;
    public bool isAndroid = false;

    bool isLeftDown = false;
    bool isRightDown = false;
    bool isJumpDown = false;
    bool isSlideDown = false;

    float steeringVelocity = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        //isAndroid = Application.platform == RuntimePlatform.Android;
        //isAndroid = true;
    }

    private void Update()
    {
        if (!isAndroid)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            jumpInput = Input.GetKey(KeyCode.Space);
            slideInput = Input.GetKeyDown(KeyCode.S);
        }

        else
        {
            horizontal = arteemHorz();
            vertical = arteemVert();
            jumpInput = isJumpDown;
            slideInput = isSlideDown;
        }
    }

    public void AxisChanged(int index)
    {
        switch (index)
        {
            case 0:
                stick.AxisOptions = AxisOptions.Both;
                background.sprite = axisSprites[index];
                break;
            case 1:
                stick.AxisOptions = AxisOptions.Horizontal;
                background.sprite = axisSprites[index];
                break;
        }
    }

    public void SnapX(bool value)
    {
        stick.SnapX = value;
    }

    void checkHorizontal()
    {
        if (isLeftDown == isRightDown) return;

        var target = 1;
        if (isLeftDown) target = -1;
        else if (isRightDown) target = 1;
        horizontal = Mathf.SmoothDamp(horizontal, target, ref steeringVelocity, 0.33f);
    }

   public void setLeft(bool b)
    {
        isLeftDown = b;
    }

    public void setRight(bool b)
    {
        isRightDown = b;
    }

    public void setJump(bool b)
    {
        isJumpDown = b;
    }

    public void setSlide(bool b)
    {
        isSlideDown = b;

        StartCoroutine(resetSlide());
    }

    IEnumerator resetSlide()
    {
        yield return new WaitForEndOfFrame();
        isSlideDown = false;
    }

    public float arteemHorz()
    {
        return stick.Horizontal;
    }

    public float arteemVert()
    {
        return stick.Vertical;
    }

}
