using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZokoAudio : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] audioClips;
    public AudioClip[] footSteps;
    public Transform gCL;
    public Transform gCC;
    public Transform gCR;
    public LayerMask groundMask;
    int prevClip = 9;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void playAudOnce(int id)
    {
        audioSource.PlayOneShot(audioClips[id]);
    }

    public void playAudLoop(int id)
    {
        if (id == 9)
        {
            if (prevClip != 9)
            {
                stopAud();
                audioSource.PlayOneShot(audioClips[id]);
            }
            else
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(audioClips[id]);
                }
            }
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioClips[id]);
            }
        }

        prevClip = id;
    }

    public void playFoot()
    {
        int id;
        bool metal = false;
        RaycastHit2D hitL = Physics2D.Raycast(gCL.position, Vector2.down, 0.2f, groundMask);
        RaycastHit2D hitC = Physics2D.Raycast(gCC.position, Vector2.down, 0.2f, groundMask);
        RaycastHit2D hitR = Physics2D.Raycast(gCR.position, Vector2.down, 0.2f, groundMask);

        if(hitL.collider != null)
        {
            if (hitL.collider.CompareTag("metal"))
            {
                metal = true;
            }
        }
        else
        {
            if(hitC.collider != null)
            {
                if (hitC.collider.CompareTag("metal"))
                {
                    metal = true;
                }
            }
            else
            {
                if(hitR.collider != null)
                {
                    if (hitR.collider.CompareTag("metal"))
                    {
                        metal = true;
                    }
                }
            }
        }

        if (!metal)
        {
            id = Random.Range(0, 4);
            audioSource.PlayOneShot(footSteps[id]);
        }
        else
        {
            id = Random.Range(4, 8);
            audioSource.PlayOneShot(footSteps[id]);
        }
    }

    public void playLand()
    {
        int id = Random.Range(8, 11);
        audioSource.PlayOneShot(footSteps[id]);
    }

    public void stopAud()
    {
        audioSource.Stop();
    }

}
