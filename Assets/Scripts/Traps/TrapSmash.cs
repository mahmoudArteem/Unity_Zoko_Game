using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSmash : MonoBehaviour
{
    public LayerMask groundMask;
    public SpriteRenderer trapMover;
    public Transform trapHeadUp;
    public Transform trapHeadDown;
    public GameObject headUpBlood;
    public GameObject headDownBlood;
    public BoxCollider2D moverCollider;
    AudioSource audioSource;
    public AudioClip[] audioClips;

    float height = 1;
    public float time = 6;
    public float countTime = 0;
    public float speed;
    public float difference;
    bool audPlayed = false;
    bool down = false;

    private void Start()
    {
        headUpBlood.SetActive(false);
        headDownBlood.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        generateTrap();
    }

    void generateTrap()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 20f, groundMask);

        if (hit.collider != null)
        {
            //heightB = hit.point.y;
            height = Mathf.Abs(transform.position.y - hit.point.y);

            trapMover.size = new Vector2(1, height - difference);
            trapHeadUp.parent = null;
            trapHeadDown.parent = null;
            trapHeadUp.position = new Vector2(transform.position.x, hit.point.y + 1);
            trapHeadDown.position = new Vector2(transform.position.x, hit.point.y);
            trapHeadUp.transform.parent = trapMover.gameObject.transform;
            trapHeadDown.parent = transform;

            if((time * 0.5f) < (height - difference))
            {
                time = Mathf.Round((height - difference));
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (countTime < time)
        {
            if (countTime < (time / 2))
            {
                trapMover.size = Vector2.MoveTowards(trapMover.size, new Vector2(1, difference + 1), Time.deltaTime * speed);

                if(trapMover.size.y > difference + 1)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(audioClips[0]);
                    }
                    audPlayed = false;

                    down = false;
                }
                else
                {
                    if (!audPlayed)
                    {
                        audioSource.Stop();
                        audPlayed = true;
                    }
                }
            }
            else
            {
                trapMover.size = Vector2.MoveTowards(trapMover.size, new Vector2(1, height), Time.deltaTime * speed);

                if (trapMover.size.y < height)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(audioClips[0]);
                    }
                    audPlayed = false;

                    down = true;
                }
                else
                {
                    if (!audPlayed)
                    {
                        audioSource.Stop();
                        audioSource.PlayOneShot(audioClips[1]);
                        audPlayed = true;
                    }
                }
            }

            trapHeadUp.localPosition = new Vector2(0, -trapMover.size.y + 1);
            moverCollider.size = new Vector2(2f, trapMover.size.y);
            moverCollider.offset = new Vector2(0, -(trapMover.size.y / 2));
            countTime += Time.deltaTime;
        }
        else
        {
            countTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (down)
        {
            if (collision.CompareTag("Player"))
            {
                headUpBlood.SetActive(true);
                headDownBlood.SetActive(true);
                collision.transform.parent.BroadcastMessage("reglarDamage", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
