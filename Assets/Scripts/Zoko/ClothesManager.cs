using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class ClothesManager : MonoBehaviour
{
    public ClothesLabels _clothesLabels;
    public SpriteResolver[] _spriteResolver;
    private int currentHat = 5;
    private void Start()
    {
        setClothes(7);
    }

    private void setClothes(int id)
    {
        for (int i = 0; i < _spriteResolver.Length; i++)
        {
            _spriteResolver[i].SetCategoryAndLabel(_clothesLabels._labelsNames.bodyParts[i], _clothesLabels._labelsNames.labelsNames[id]);
        }

    }

    public int getHat()
    {
        return currentHat;
    }
}
