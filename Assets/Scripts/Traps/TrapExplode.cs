using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapExplode : MonoBehaviour
{
    public SpriteRenderer berrelRenderer;
    public float _time;
    public float radius;
    public Color secondColor;
    public LayerMask explodeMask;
    public LayerMask groundMask;
    public GameObject explosionVFX;
    float countTime = 0;
    bool trigger = false;
    Color firstColor;
    AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject flame;

    private void Start()
    {
        firstColor = berrelRenderer.color;
        secondColor = new Color(secondColor.r, secondColor.g, secondColor.b, 1);
        audioSource = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if (trigger)
        {
            if(countTime < _time)
            {
                berrelRenderer.color = Color.Lerp(firstColor, secondColor, countTime / _time);

                countTime += Time.deltaTime;
            }
            else
            {
                distanceExplode();
                trigger = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            trigger = true;
            flame.SetActive(true);
        }

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    void distanceExplode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, explodeMask);

        if(colliders.Length > 0)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                Vector2 p = new Vector2(colliders[i].transform.position.x, colliders[i].transform.position.y + 0.2f);
                if (!Physics2D.Linecast(transform.position, p, groundMask))
                {
                    colliders[i].transform.parent.BroadcastMessage("explodeMe", transform.position.x, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void explodeMe()
    {
        StartCoroutine(endMe());
    }

    IEnumerator endMe()
    {
        yield return new WaitForSeconds(0.5f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, explodeMask);

        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                Vector2 p = new Vector2(colliders[i].transform.position.x, colliders[i].transform.position.y + 0.2f);
                if (!Physics2D.Linecast(transform.position, p, groundMask))
                {
                    colliders[i].transform.parent.BroadcastMessage("explodeMe", transform.position.x, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
