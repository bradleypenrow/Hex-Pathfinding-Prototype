using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Primary class of each Hex. Each province is randomly generated to have 
/// a different terrain, which impacts travel time.
/// </summary>
public class Province : MonoBehaviour, IHeapItem<Province>
{
    public Terrain terrain;
    public Resource resource;
    public int SupplyLimit;
    public int provinceLevel;
    public bool isCapital;
    public bool adjacentRiver;
    public Vector2 coord;
    public Vector3 cubicCoord;
    public bool riverMouth = false;
    public Province[] neighbors = new Province[6];

    public float gCost;
    public float hCost;

    public Province parent;

    int heapIndex;

    public float fCost { get { return gCost + hCost; } }

    //Methods used for Heap
    #region Heap Methods
    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    //Used in Heap to determine smallest item 
    public int CompareTo(Province toCompare)
    {
        int compare = fCost.CompareTo(toCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(toCompare.hCost);
        }
        return -compare;
    }
    #endregion
}
