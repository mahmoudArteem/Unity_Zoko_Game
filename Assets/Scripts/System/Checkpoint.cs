using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    bool checkedMe = false;
    Animator anim;

    private void Start()
    {
        Vector2 managerPos = GameManager.Instance.getCheckPoint();

        if(Mathf.Abs(transform.position.x - managerPos.x) < 0.1f)
        {
            checkedMe = true;
        }
        else
        {
            checkedMe = false;
        }

        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!checkedMe)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance.setCheckPoint(transform.position);

                anim.SetBool("Checked", true);

                checkedMe = true;
            }
        }
    }
}
