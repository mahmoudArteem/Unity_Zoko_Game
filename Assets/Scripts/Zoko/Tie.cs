using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tie : MonoBehaviour
{
    public Animator tieAnimator;

    private void Update()
    {
        tieAnimator.SetBool("Ground", ZokoAnimManager.Instance.getGround());
        tieAnimator.SetFloat("VerSpeed", ZokoAnimManager.Instance.getAnimSpeed().y);
        tieAnimator.SetBool("Look", ZokoAnimManager.Instance.getLook());
    }
}
