using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBtn : MonoBehaviour
{
    RectTransform myRect;
    Vector2 startPos;
    Vector2 endPos;
    float _time = 8;
    float countedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
        startPos = new Vector2(-myRect.rect.width / 2, myRect.rect.height);
        endPos = new Vector2(myRect.rect.width + (myRect.rect.width / 2), -myRect.rect.height);
        transform.localPosition = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(countedTime < _time)
        {
            if(countedTime < (_time / 2))
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, endPos, 10);
            }
            else{

                transform.localPosition = Vector2.MoveTowards(transform.localPosition, startPos, 10);
            }

            countedTime += Time.deltaTime;
        }
        else
        {
            countedTime = 0;
        }
    }

}
