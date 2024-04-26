using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.loadScene(1);
    }

}
