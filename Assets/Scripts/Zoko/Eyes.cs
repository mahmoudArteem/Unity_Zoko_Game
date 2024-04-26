using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    public Animator eyeAnimator;
    private void Update()
    {
        eyeAnimator.SetFloat("HorzSpeed", ZokoAnimManager.Instance.getAnimSpeed().x);
        eyeAnimator.SetFloat("VerSpeed", Mathf.Abs(ZokoAnimManager.Instance.getAnimSpeed().y));
        
    }

    public void setLook(int b)
    {
        if (b == 1)
        {
            eyeAnimator.SetBool("Look", true);
            ZokoAnimManager.Instance.setLook(true);
        }
        else
        {
            eyeAnimator.SetBool("Look", false);
            ZokoAnimManager.Instance.setLook(false);
        }
    }
}
