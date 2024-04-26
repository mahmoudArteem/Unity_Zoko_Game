using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using UnityEditorInternal;

public class LayersId : MonoBehaviour
{
    string[] names;
    int[] ids;

    private void Start()
    {
        names = GetSortingLayerNames();
        ids = GetSortingLayerUniqueIDs();

        for(int i = 0; i < names.Length; i++)
        {
            Debug.Log("Layer name : " + names[i] + ", ID = " + ids[i]);
        }
    }
    string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }
    int[] GetSortingLayerUniqueIDs(bool sortNum = false)
    {
        System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
        int[] idArray = (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
        if (sortNum)
        {
            Array.Sort(idArray);
        }
        return idArray;
    }
}
