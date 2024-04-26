using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSwing : MonoBehaviour
{
    public float angle;
    public float speed;
    public Transform leftScrew;
    public Transform rightScrew;
    public SpriteRenderer leftChain;
    public SpriteRenderer rightChain;
    Quaternion startAngle;
    Quaternion endAngle;
    float startTime = 0;
    float dir = 1;
    float currentX;
    public Transform handle;
    AudioSource audioSource;
    public AudioClip trapAud;
    bool playedR;
    bool playedL;
    public SwingTrigger swingTriggerL;
    public SwingTrigger swingTriggerR;
    private void Start()
    {
        startAngle = pendulum(angle);
        endAngle = pendulum(-angle);
        dir = transform.position.x;
        currentX = handle.position.x;
        audioSource = GetComponent<AudioSource>();

        //generatChain();
    }

    private void FixedUpdate()
    {
        startTime += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(startAngle, endAngle, (Mathf.Sin(startTime * speed + Mathf.PI / 2) + 1.0f) / 2.0f);

        if(currentX < handle.position.x)
        {
            dir = 1;
            if (!playedR)
            {
                swingTriggerL.setDir(dir);
                swingTriggerR.setDir(dir);
                audioSource.PlayOneShot(trapAud);
                playedL = false;
                playedR = true;
            }
        }
        else
        {
            dir = -1;
            if (!playedL)
            {
                swingTriggerL.setDir(dir);
                swingTriggerR.setDir(dir);
                audioSource.PlayOneShot(trapAud);
                playedR = false;
                playedL = true;
            }
        }

        currentX = handle.position.x;
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            blood.SetActive(true);
            collision.transform.parent.BroadcastMessage("pushingDamage", dir, SendMessageOptions.DontRequireReceiver);
        }
    }*/

    Quaternion pendulum(float angle)
    {
        var pendulumRotation = transform.rotation;
        var angleZ = pendulumRotation.eulerAngles.z + angle;

        if(angleZ > 180)
        {
            angleZ -= 360;
        }
        else
        {
            if(angleZ < -180)
            {
                angleZ += 360;
            }
        }

        pendulumRotation.eulerAngles = new Vector3(0, 0, angleZ);

        return pendulumRotation;
    }

    public void generatChain()
    {
        Vector3 directionR = transform.position - rightScrew.position;
        directionR = Vector3.Normalize(directionR);
        Vector3 directionL = transform.position - leftScrew.position;
        directionL = Vector3.Normalize(directionL);

        rightChain.transform.up = directionR;
        float scaleR = Vector3.Distance(transform.position, rightScrew.position);
        rightChain.size = new Vector2(rightChain.size.x, scaleR);

        leftChain.transform.up = directionL;
        leftChain.size = new Vector2(leftChain.size.x, scaleR);
    }
}
