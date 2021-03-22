using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Manages all path requests
/// Future enhancements would allow processing of multiple paths at once.
/// </summary>
public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;
    bool isProcessingPath;

    public void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    //Method for army to call to request a path
    public static void RequestPath(Province pathStart, Province pathEnd, Action<Province[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    //Send path back to army that requested it, and move on to the next item in the queue
    public void FinishedProcessingPath(Province[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Province pathStart;
        public Province pathEnd;
        public Action<Province[], bool> callback;

        public PathRequest(Province _start, Province _end, Action<Province[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
