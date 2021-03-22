using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainData", menuName = "ScriptableObjects/TerrainObject", order = 1)]
public class Terrain : ScriptableObject
{
    public string terrainType;
    public float travelTime;
    public float defensiveBonus;
    public bool traversable = true;
    public float spawnChance;
}
