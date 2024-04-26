using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapStar : MonoBehaviour
{
    //public GameObject blood;
    public float speed;
    Rigidbody2D rig;
    float direction = 1;

    private void Start()
    {
        //blood.SetActive(false);
        rig = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //rig.velocity = new Vector2(0, speed * direction);
        //transform.Rotate(0, 0, 15);

        rig.velocity = transform.up * -speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //blood.SetActive(true);
            collision.transform.parent.BroadcastMessage("reglarDamage", SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void setStar(float dir, float s)
    {
        direction = dir;
        speed = s;
    }
}
