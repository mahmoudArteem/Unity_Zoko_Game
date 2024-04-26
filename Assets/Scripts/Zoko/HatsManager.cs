using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HatsData", menuName = "Arteem/Hats Data", order = 5)]
public class HatsManager : ScriptableObject
{
    public List<HatData> hatsData = new List<HatData>();
}
