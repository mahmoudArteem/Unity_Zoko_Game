using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : MonoBehaviour
{
    public ParticleSystem fireParticle;
    public Transform head;
    public float _time;
    float countTime;
    bool enableCollider = false;
    float dir = 1;
    AudioSource audioSource;

    private void Start()
    {
        fireParticle.Stop();
        audioSource = GetComponent<AudioSource>();
        if (transform.eulerAngles.z < 180)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
    }

    private void FixedUpdate()
    {
        if (countTime < _time)
        {
            if (countTime < (_time / 2))
            {
                if (fireParticle.isPlaying)
                {
                    audioSource.Stop();
                    fireParticle.Stop();
                }
                enableCollider = false;
            }
            else
            {
                if (fireParticle.isStopped)
                {
                    audioSource.Play();
                    fireParticle.Play();
                }
                enableCollider = true;
            }

            countTime += Time.deltaTime;
        }
        else
        {
            countTime = 0;
        }

        if (enableCollider)
        {
            RaycastHit2D hit = Physics2D.Raycast(head.position, dir * Vector2.right, 2f);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("hited");
                    hit.collider.transform.parent.BroadcastMessage("fireMe", SendMessageOptions.DontRequireReceiver);
                }
            }

        }
    }


}
