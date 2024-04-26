using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFall : MonoBehaviour
{
    public SpriteRenderer brickRenderer;
    public GameObject dirtPre;
    public float _time;
    float countedTime;
    float timeDiff = 0;
    int step = 0;
    public Sprite[] brickSprites;
    bool trigger = false;
    float timeAdded = 0;
    bool dirtCreated = false;

    private void Start()
    {
        timeAdded = _time / brickSprites.Length;
        timeDiff = timeAdded;
    }
    private void FixedUpdate()
    {
        if (trigger)
        {
            if(countedTime < _time)
            {
                if(countedTime > timeDiff)
                {
                    if(step < brickSprites.Length)
                    {
                        brickRenderer.sprite = brickSprites[step];
                    }

                    step++;

                    timeDiff += timeAdded;
                }

                countedTime += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            trigger = true;

            if (!dirtCreated)
            {
                GameObject dirt = Instantiate(dirtPre, transform.position, Quaternion.identity);
                dirt.transform.parent = transform;
                dirt.transform.localPosition = new Vector2(0, 0.5f);
                dirtCreated = true;
            }
        }
    }
}
