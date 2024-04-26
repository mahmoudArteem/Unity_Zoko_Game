using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Transform gearRTop;
    public Transform gearRDown;
    public Transform gearLTop;
    public Transform gearLDown;
    public SpriteRenderer ropeL;
    public SpriteRenderer ropeR;
    public Transform elevator;
    public float time = 6;
    public float speed = 2;
    float countTime = 0;

    private void FixedUpdate()
    {
        if (countTime < time)
        {
            if (countTime < (time / 2))
            {
                elevator.localPosition = Vector2.MoveTowards(elevator.localPosition,
                           new Vector2(elevator.localPosition.x, gearRDown.localPosition.y), Time.deltaTime * speed);
            }
            else
            {
                elevator.localPosition = Vector2.MoveTowards(elevator.localPosition,
                           new Vector2(elevator.localPosition.x, 0), Time.deltaTime * speed);

            }

            countTime += Time.deltaTime;
        }
        else
        {
            countTime = 0;
        }
    }
    public void generateElevator()
    {
        gearRTop.localPosition = new Vector3(gearRTop.localPosition.x, 0, 0);
        gearLTop.localPosition = new Vector3(-gearRTop.localPosition.x, 0, 0);
        gearLDown.localPosition = new Vector3(-gearRDown.localPosition.x, gearRDown.localPosition.y, 0);
        float height = Mathf.Abs(gearRTop.position.y - gearRDown.position.y);

        ropeL.size = new Vector2(0.64f, height);
        ropeR.size = new Vector2(0.64f, height);

        if ((time * 0.5f) < height)
        {
            time = Mathf.Round(height);
        }
    }
}
