using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "ScriptableObjects/ResourceObject", order = 2)]
public class Resource : ScriptableObject
{
    public string resourceName;
    public int resourceValue;
}
