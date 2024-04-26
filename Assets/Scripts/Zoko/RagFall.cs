using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagFall : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
