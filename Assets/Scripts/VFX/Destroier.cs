using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroier : MonoBehaviour
{
    public float seconds;

    private void Start()
    {
        StartCoroutine(destroyMe());
    }

    IEnumerator destroyMe()
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
