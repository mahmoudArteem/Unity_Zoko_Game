using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingTrigger : MonoBehaviour
{
    public GameObject blood;
    float dir;

    private void Start()
    {
        blood.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            blood.SetActive(true);
            collision.transform.parent.BroadcastMessage("pushingDamage", dir, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void setDir(float d)
    {
        dir = d;
    }
}
