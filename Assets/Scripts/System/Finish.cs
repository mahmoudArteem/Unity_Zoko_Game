using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    bool finishedMe = false;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!finishedMe)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance.setFinished(true);
                collision.transform.parent.BroadcastMessage("setFinished", SendMessageOptions.DontRequireReceiver);
                
                anim.SetBool("Checked", true);

                finishedMe = true;
            }
        }
    }
}
