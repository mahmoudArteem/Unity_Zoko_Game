using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorGenerator : MonoBehaviour
{
    public SpriteRenderer leftColoumn;
    public SpriteRenderer rightColoumn;
    public Transform knifes;
    public Transform node;
    public Transform elevatorPos;
    public GameObject elevatorPrefUp;
    public GameObject elevatorPrefDown;

    public void generateTrap()
    {
        float height = Mathf.Abs(transform.position.y - knifes.position.y) + 1f;
        float yPos = (transform.position.y + knifes.position.y) / 2;

        leftColoumn.size = new Vector2(0.5f, height);
        rightColoumn.size = new Vector2(0.5f, height);

        leftColoumn.transform.position = new Vector2(leftColoumn.transform.position.x, yPos + 0.5f);
        rightColoumn.transform.position = new Vector2(rightColoumn.transform.position.x, yPos + 0.5f);

        elevatorPos.localPosition = new Vector2(0, 1);
        transform.eulerAngles = new Vector3(0, 0, 0);
        elevatorPos.localEulerAngles = new Vector3(0, 0, 0);

        relaunche();

    }

    public void relaunche()
    {
        if((elevatorPos.position.y - knifes.position.y) > 0)
        {
            GameObject ed = Instantiate(elevatorPrefDown, elevatorPos.position, Quaternion.identity);
        }
        else
        {
            GameObject eu = Instantiate(elevatorPrefUp, elevatorPos.position, Quaternion.identity);
        }
        //GameObject el = Instantiate(elevatorPref, elevatorPos.position, Quaternion.identity);

    }

}
