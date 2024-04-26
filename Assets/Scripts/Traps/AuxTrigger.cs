using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxTrigger : MonoBehaviour
{
    public GameObject blood;
    float dir = 1;
    TrapAux at;
    private void Start()
    {
        at = GetComponentInParent<TrapAux>();
        blood.SetActive(false);
        dir = Mathf.Sign(at.speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            blood.SetActive(true);
            collision.transform.parent.BroadcastMessage("pushingDamage", dir, SendMessageOptions.DontRequireReceiver);
        }
    }
}
