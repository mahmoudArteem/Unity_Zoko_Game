using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagAudio : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] screams;
    public AudioClip[] trapScreams;
    public AudioClip laserBurn;
    int screamCounted = 0;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playScream()
    {
        int id = Random.Range(0, 5);

        if (!audioSource.isPlaying)
        {
            screamCounted += 1;
            audioSource.PlayOneShot(screams[id]);
            StartCoroutine(repeatScream());
        }
    }

    IEnumerator repeatScream()
    {
        yield return new WaitForSeconds(2);

        if (screamCounted < 2)
        {
            playScream();
        }
    }

    public void stopScream()
    {
        audioSource.Stop();
    }

    public void playAudRandom()
    {
        int id = Random.Range(0, 7);
        audioSource.PlayOneShot(trapScreams[id]);
    }

    public void playLaser()
    {
        audioSource.PlayOneShot(laserBurn);
        audioSource.PlayOneShot(trapScreams[0]);
    }
}
