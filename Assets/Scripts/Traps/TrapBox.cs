using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBox : MonoBehaviour
{
    public GameObject top;
    public GameObject down;
    public GameObject right;
    public GameObject left;

    public bool topTrap;
    public bool downTrap;
    public bool rightTrap;
    public bool leftTrap;

    public bool rotating;
    public float speed;

    private void Start()
    {
        if (!topTrap)
        {
            Destroy(top);
        }
        if (!downTrap)
        {
            Destroy(down);
        }
        if (!rightTrap)
        {
            Destroy(right);
        }
        if (!leftTrap)
        {
            Destroy(left);
        }
    }

    private void FixedUpdate()
    {
        if (rotating)
        {
            transform.Rotate(0, 0, speed);
        }
    }
    public void generateTrap()
    {
        if (topTrap)
        {
            top.SetActive(true);
        }
        else
        {
            top.SetActive(false);
        }

        if (downTrap)
        {
            down.SetActive(true);
        }
        else
        {
            down.SetActive(false);
        }

        if (rightTrap)
        {
            right.SetActive(true);
        }
        else
        {
            right.SetActive(false);
        }

        if (leftTrap)
        {
            left.SetActive(true);
        }
        else
        {
            left.SetActive(false);
        }
    }
}
