using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLaserHorz : MonoBehaviour
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
        float ang = transform.localEulerAngles.z;
        if(ang > 180)
        {
            ang = ang - 360;
        }

        if (ang < 0)
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
        distA = Mathf.Abs(transform.position.y - pointA.position.y);
        distB = Mathf.Abs(transform.position.y - pointB.position.y);
        if (moveToB)
        {
            if (distB < 0.1f)
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir * Vector2.right, 20f);

        if (hit.collider != null)
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
            beam.SetPosition(1, new Vector2(transform.position.x + (dir * 20), transform.position.y));
        }

    }
}
