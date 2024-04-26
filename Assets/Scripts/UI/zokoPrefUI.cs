using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zokoPrefUI : MonoBehaviour
{
    public Animator[] anim;

    private void Start()
    {
        anim = GetComponentsInChildren<Animator>();
    }

    private void OnEnable()
    {

        StartCoroutine(sendAnim());
    }

    IEnumerator sendAnim()
    {
        yield return new WaitForEndOfFrame();

        ZokoAnimManager.Instance.setLook(false);
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].Play(0, 0, 0);
        }
    }
}
