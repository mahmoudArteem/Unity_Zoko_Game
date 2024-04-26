using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    public GameObject trapBlood;

    private void Start()
    {
        trapBlood.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            trapBlood.SetActive(true);
            collision.transform.parent.BroadcastMessage("reglarDamage", SendMessageOptions.DontRequireReceiver);
        }
    }
}
