using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBorderHorz : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public SpriteRenderer border;
    public Transform laserBeam;
    float distance;

    public void generateTrapBorder()
    {
        pointA.position = new Vector2(transform.position.x, pointA.position.y);
        pointB.position = new Vector2(transform.position.x, pointB.position.y);

        distance = Mathf.Abs(pointA.position.y - pointB.position.y);

        border.transform.position = (pointA.position + pointB.position) / 2;
        border.size = new Vector2(0.64f, distance);//change this to math the sprite size
        laserBeam.transform.position = (pointA.position + pointB.position) / 2;
    }

    public void flip()
    {
        if (laserBeam.eulerAngles.z > 180)
        {
            laserBeam.localEulerAngles = new Vector3(0, 0, 90);
            //laserBeam.position = new Vector2(laserBeam.position.x, laserBeam.position.y + offset);
        }
        else
        {
            laserBeam.localEulerAngles = new Vector3(0, 0, -90);
            //laserBeam.position = new Vector2(laserBeam.position.x, laserBeam.position.y + offset);
        }

        generateTrapBorder();
    }
}
