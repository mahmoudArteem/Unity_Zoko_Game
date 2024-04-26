using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{

    public ElevatorGenerator _elevatorGenerator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent.BroadcastMessage("electricMe", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            if (collision.CompareTag("Elevator"))
            {
                Destroy(collision.gameObject);
                _elevatorGenerator.relaunche();
            }
        }
    }
}
