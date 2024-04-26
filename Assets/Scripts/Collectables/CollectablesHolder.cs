using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesHolder : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject[] items;
    void Start()
    {
        items = new GameObject[transform.childCount];

        setChilds();
        updateItems();
    }

    void setChilds()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            items[i] = transform.GetChild(i).gameObject;
        }
    }

    void updateItems()
    {
        bool[] b = GameManager.Instance.getCollectables();

        if (b.Length > 0)
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].SetActive(b[i]);
            }
        }
        else
        {
            bool[] c = new bool[items.Length];

            for(int i = 0; i < c.Length; i++)
            {
                c[i] = true;
            }

            GameManager.Instance.setCollectables(c);
        }
    }


}
