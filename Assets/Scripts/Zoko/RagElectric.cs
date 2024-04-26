using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagElectric : MonoBehaviour
{
    public Transform hip;
    void LateUpdate()
    {
        transform.position = new Vector2(hip.position.x, hip.position.y - 0.1f);
    }
}
