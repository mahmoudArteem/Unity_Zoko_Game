using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZokoVFX : MonoBehaviour
{
    public GameObject smokePref;
    public Transform smokePos;
    public ParticleSystem smokeSlide;
    public ParticleSystem smokeWall;

    private void Start()
    {
        smokeSlide.Stop();
        smokeWall.Stop();
    }
    public void createVFX(int id)
    {
        switch (id)
        {
            case 0:
                Instantiate(smokePref, smokePos.position, Quaternion.identity);
                break;
            case 1:
                if (!smokeSlide.isPlaying)
                {
                    smokeSlide.Play();
                }
                break;
            case 2:
                if (!smokeWall.isPlaying)
                {
                    smokeWall.Play();
                }
                break;
        }
    }

    public void stopVFX(int id)
    {
        switch (id)
        {
            case 1:
                if (smokeSlide.isPlaying)
                {
                    smokeSlide.Stop();
                }
                break;
            case 2:
                if (smokeWall.isPlaying)
                {
                    smokeWall.Stop();
                }
                break;
        }
    }
}
