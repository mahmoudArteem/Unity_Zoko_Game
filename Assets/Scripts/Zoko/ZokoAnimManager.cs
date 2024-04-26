using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZokoAnimManager : MonoBehaviour
{
    public static ZokoAnimManager Instance;

    Vector2 animSpeed;
    bool grounded;
    bool look;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void setAnimSpeed(float h, float v)
    {
        animSpeed = new Vector2(h, v);
    }

    public Vector2 getAnimSpeed()
    {
        return animSpeed;
    }

    public void setGround(bool g)
    {
        grounded = g;
    }

    public bool getGround()
    {
        return grounded;
    }

    public void setLook(bool l)
    {
        look = l;
    }

    public bool getLook()
    {
        return look;
    }
}
