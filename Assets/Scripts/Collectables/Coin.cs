using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    public int amount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.addCoin(amount, transform.GetSiblingIndex());

            //Destroy(gameObject);
            gameObject.SetActive(false);

        }
    }
}
