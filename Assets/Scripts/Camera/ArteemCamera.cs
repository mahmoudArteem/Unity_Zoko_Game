using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArteemCamera : MonoBehaviour
{
    public Transform movingBackgrounds;

    private void Start()
    {

        movingBackgrounds.parent = null;
    }

    private void Update()
    {
        movingBackgrounds.position = new Vector2(movingBackgrounds.position.x, transform.position.y);
    }
}
