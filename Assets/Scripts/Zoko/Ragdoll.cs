using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody2D[] rigs;
    public Rigidbody2D hip;
    public GameObject fire;
    public GameObject electricity;
    public GameObject blood;
    public RagAudio ragAudio;
    bool doPush = false;
    float direction;
    private void FixedUpdate()
    {
        if (doPush)
        {
            for (int i = 0; i < rigs.Length; i++){
                //rigs[i].drag = 0;
                rigs[i].AddForce(new Vector2(direction * 100, 100), ForceMode2D.Impulse);
            }

            //hip.AddForce(new Vector2(direction * 300, 300), ForceMode2D.Impulse);
            doPush = false;
        }
    }


    public void pushme(float dir)
    {
        direction = dir;
        doPush = true;
        
    }

    public void fireMe()
    {
        fire.SetActive(true);
        fire.GetComponent<ParticleSystem>().Play();
        //fire.GetComponent<AudioSource>().Play();
        ragAudio.playScream();
    }

    public void electricMe()
    {
        electricity.SetActive(true);
        electricity.GetComponent<ParticleSystem>().Play();
        ragAudio.playScream();
    }

    public void laserMe()
    {
        ragAudio.playLaser();
    }

    public void doBlood()
    {
        Instantiate(blood, hip.transform.position, Quaternion.identity);
        ragAudio.playAudRandom();
    }


}
