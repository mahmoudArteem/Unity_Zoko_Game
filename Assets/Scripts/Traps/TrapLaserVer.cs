using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLaserVer : MonoBehaviour
{
    public LineRenderer beam;
    float dir = 1;
    public Transform pointA;
    public Transform pointB;
    public float speed;
    bool moveToB = true;
    float distA;
    float distB;

    private void Start()
    {
        beam.useWorldSpace = true;
        if(transform.eulerAngles.z < 180)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }

    }

    private void Update()
    {
        distA = Mathf.Abs(transform.position.x - pointA.position.x);
        distB = Mathf.Abs(transform.position.x - pointB.position.x);
        if (moveToB)
        {
            if(distB < 0.1f)
            {
                moveToB = false;
            }
        }
        else
        {
            if (distA < 0.1f)
            {
                moveToB = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (moveToB)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, pointB.localPosition, Time.deltaTime * speed);
        }
        else
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, pointA.localPosition, Time.deltaTime * speed);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir * Vector2.up, 20f);

        if(hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.transform.parent.BroadcastMessage("laserMe", SendMessageOptions.DontRequireReceiver);
                beam.SetPosition(0, transform.position);
                beam.SetPosition(1, hit.point);
            }
            else
            {
                beam.SetPosition(0, transform.position);
                beam.SetPosition(1, hit.point);
            }
        }
        else
        {
            beam.SetPosition(0, transform.position);
            beam.SetPosition(1, new Vector2(transform.position.x, transform.position.y + (dir * 20f)));
        }

    }

}
