using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public GameObject ragdoll;
    float myPos;
    bool created = false;
    /*private void Update()
    {
        myPos = transform.position.x;
    }*/
    public void reglarDamage()
    {
        if (!created)
        {
            GameObject rag = Instantiate(ragdoll, transform.position, Quaternion.identity);
            rag.BroadcastMessage("doBlood", SendMessageOptions.DontRequireReceiver);
            created = true;
        }

        GameManager.Instance.setDead(true);
        Destroy(gameObject);
    }

    public void pushingDamage(float pos)
    {
        if (!created)
        {
            GameObject rag = Instantiate(ragdoll, transform.position, Quaternion.identity);

            if (pos > 0)
            {
                rag.BroadcastMessage("pushme", 1, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                rag.BroadcastMessage("pushme", -1, SendMessageOptions.DontRequireReceiver);
            }

            rag.BroadcastMessage("doBlood", SendMessageOptions.DontRequireReceiver);
            created = true;

        }
        GameManager.Instance.setDead(true);
        Destroy(gameObject);
    }

    public void explodeMe(float pos)
    {
        if (!created)
        {
            myPos = transform.position.x;

            GameObject rag = Instantiate(ragdoll, transform.position, Quaternion.identity);
            if (pos < myPos)
            {
                rag.BroadcastMessage("pushme", 1, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                rag.BroadcastMessage("pushme", -1, SendMessageOptions.DontRequireReceiver);
            }
            created = true;
        }
        GameManager.Instance.setDead(true);
        Destroy(gameObject);
    }

    public void fireMe()
    {
        GameObject rag =  Instantiate(ragdoll, transform.position, Quaternion.identity);
        rag.BroadcastMessage("fireMe", SendMessageOptions.DontRequireReceiver);

        GameManager.Instance.setDead(true);
        Destroy(gameObject);
    }

    public void electricMe()
    {
        GameObject rag = Instantiate(ragdoll, transform.position, Quaternion.identity);
        rag.BroadcastMessage("electricMe", SendMessageOptions.DontRequireReceiver);

        GameManager.Instance.setDead(true);
        Destroy(gameObject);
    }

    public void laserMe()
    {
        GameObject rag = Instantiate(ragdoll, transform.position, Quaternion.identity);
        rag.BroadcastMessage("laserMe", SendMessageOptions.DontRequireReceiver);

        GameManager.Instance.setDead(true);
        Destroy(gameObject);
    }

}
