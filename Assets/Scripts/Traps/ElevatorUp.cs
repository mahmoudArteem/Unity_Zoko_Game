using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUp : MonoBehaviour
{
    Rigidbody2D rig;

    public float speed = 2;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector2(0, speed);

    }
}
