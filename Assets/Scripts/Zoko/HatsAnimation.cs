using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatsAnimation : MonoBehaviour
{
    public Animator hatAnimator;
    public SpriteRenderer sr;
    public HatsManager hm;
    public ClothesManager _clothesManager;
    private void Update()
    {
        hatAnimator.SetBool("Ground", ZokoAnimManager.Instance.getGround());
        hatAnimator.SetFloat("VerSpeed", ZokoAnimManager.Instance.getAnimSpeed().y);
        hatAnimator.SetBool("Look", ZokoAnimManager.Instance.getLook());
    }

    public void setHatValue(int value)
    {
        int hatId = _clothesManager.getHat();
        transform.localPosition = hm.hatsData[hatId].position;

        switch (value)
        {
            case 0: sr.sprite = hm.hatsData[hatId].front;
                break;
            case 1: sr.sprite = hm.hatsData[hatId].quarter;
                break;
            case 2: sr.sprite = hm.hatsData[hatId].side;
                break;
            default: sr.sprite = hm.hatsData[hatId].side;
                break;
        }
    }
}
