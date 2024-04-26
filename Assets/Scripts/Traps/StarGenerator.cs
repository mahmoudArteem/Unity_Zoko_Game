using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{
    public GameObject star;
    public float _time;
    int down;
    public float speed;
    float countTime;
    AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        if(transform.eulerAngles.z < 180)
        {
            down = -1;
        }
        else
        {
            down = 1;
        }

        audioSource = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if(countTime < _time)
        {
            countTime += Time.deltaTime;
        }
        else
        {
            GameObject s = Instantiate(star, transform.position, transform.rotation);
            s.GetComponent<TrapStar>().setStar(down, speed);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioClip);
            }
            countTime = 0;
        }
    }
}
