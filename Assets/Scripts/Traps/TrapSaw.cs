using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSaw : MonoBehaviour
{
    public Transform blade;
    public GameObject blood;
    public float speed;

    private void Start()
    {
        blood.SetActive(false);
    }
    private void FixedUpdate()
    {
        blade.Rotate(0, 0, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            blood.SetActive(true);
            collision.transform.parent.BroadcastMessage("reglarDamage", SendMessageOptions.DontRequireReceiver);
        }
    }
}
