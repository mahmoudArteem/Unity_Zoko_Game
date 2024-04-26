using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAux : MonoBehaviour
{
    public float speed;

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, speed);
        //transform.RotateAroundLocal(Vector3.forward, Time.deltaTime * speed);
    }
}
