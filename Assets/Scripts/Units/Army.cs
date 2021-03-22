using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Army is comprised of units.
/// Each army handles its pathfinding independently of other armies.
/// </summary>
public class Army : MonoBehaviour
{
    public Unit[] units;

    public Province currentProvince;
    public Province nextDestinationProvince;
    public Province targetProvince;
    float speed = 20;
    Province[] path;
    int targetIndex;

    //Request a new path
    public void RequestArmyPath(Province target)
    {
        PathRequestManager.RequestPath(currentProvince, target, OnPathFound);
    }

    //If path is found, cancel our current path and start our new one.
    public void OnPathFound(Province[] newPath, bool pathFound)
    {
        if (pathFound)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");            
            StartCoroutine("FollowPath");
        }
    }

    //Coroutine that instructs our army to follow the path.
    IEnumerator FollowPath()
    {
        Province currentWaypoint = path[0];
        while (true)
        {
            //If reached target province, move on to the next one
            if (transform.position.x == currentWaypoint.transform.position.x && transform.position.z == currentWaypoint.transform.position.z)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    //End of path. Break coroutine
                    path = null;
                    currentProvince = currentWaypoint;
                    yield break;
                }
                currentProvince = currentWaypoint;
                currentWaypoint = path[targetIndex];
                
            }
            //move towards target 
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentWaypoint.transform.position.x,transform.position.y, currentWaypoint.transform.position.z), speed * Time.deltaTime);
            yield return null;
        }
    }

    //Displays the army's path while in Scene mode
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(path[i].transform.position, Vector3.one);
                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i].transform.position);
                }
                else
                    Gizmos.DrawLine(path[i - 1].transform.position, path[i].transform.position);
            }

        }
    }

}
