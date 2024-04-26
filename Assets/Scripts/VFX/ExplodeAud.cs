using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAud : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] explosionAudios;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        int id = Random.Range(0, 3);

        audioSource.PlayOneShot(explosionAudios[id]);
    }
}
