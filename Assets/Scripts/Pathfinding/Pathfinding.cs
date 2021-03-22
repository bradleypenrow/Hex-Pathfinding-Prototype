using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Diagnostics;
using System;

/// <summary>
/// A* Pathfinding Algorithm
/// </summary>
public class Pathfinding : MonoBehaviour
{
    public InitializeMap map;
    PathRequestManager requestManager;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Province pathStart, Province pathEnd)
    {
        StartCoroutine(FindPath(pathStart, pathEnd));
    }

    IEnumerator FindPath(Province startProvince, Province targetProvince)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        
        Province[] waypoints = new Province[0];
        bool pathFound = false;

        if (startProvince.terrain.traversable && targetProvince.terrain.traversable)
        {

            Heap<Province> openSet = new Heap<Province>(map.rows * map.cols);
            HashSet<Province> closedSet = new HashSet<Province>();

            openSet.Add(startProvince);

            while (openSet.Count > 0)
            {
                Province currentProvince = openSet.RemoveFirst();

                closedSet.Add(currentProvince);

                if (currentProvince == targetProvince)
                {
                    //Found path!                
                    sw.Stop();
                    UnityEngine.Debug.Log("Path found " + sw.ElapsedMilliseconds + " ms");
                    pathFound = true;
                    break;
                }

                foreach (Province neighbor in currentProvince.neighbors)
                {
                    if (neighbor != null)
                    {
                        if (!neighbor.terrain.traversable || closedSet.Contains(neighbor))
                        {
                            continue;
                        }

                        //Compare the distance, and the cost of the province
                        float newMovementCostToNeighbor = currentProvince.gCost + GetDistance(currentProvince, neighbor) + neighbor.terrain.travelTime;
                        if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                        {
                            neighbor.gCost = newMovementCostToNeighbor;
                            neighbor.hCost = GetDistance(neighbor, targetProvince);
                            neighbor.parent = currentProvince;

                            if (!openSet.Contains(neighbor))
                                openSet.Add(neighbor);
                            else
                                openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathFound)
        {
            waypoints = RetracePath(startProvince, targetProvince);
        }
        requestManager.FinishedProcessingPath(waypoints, pathFound);
    }


    //Retrace Path by looking at parents of end node until you reach the start
    Province[] RetracePath(Province start, Province end)
    {
        List<Province> path = new List<Province>();
        Province currentNode = end;
        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path.ToArray();
    }

    float GetDistance(Province a, Province b)
    {
        return Mathf.Max(Mathf.Abs(a.cubicCoord.x - b.cubicCoord.x), Mathf.Abs(a.cubicCoord.y - b.cubicCoord.y), Mathf.Abs(a.cubicCoord.z - b.cubicCoord.z));
    }
}
