using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathFinderController : MonoBehaviour
    {
        private readonly Queue<PathResult> results = new Queue<PathResult>();

        // static PathRequestManager instance;
        // Pathfinding pathfinding;

        private void Update() 
        {
            if (results.Count > 0) 
            {
                int _itemsInQueue = results.Count;
                
                lock (results) 
                {
                    for(var _i = 0; _i < _itemsInQueue; _i++) 
                    {
                        PathResult _result = results.Dequeue();
                        
                        _result.OnPathFound(_result.Path, _result.Success);
                    }
                }
            }
        }

        public void RequestPath(PathRequest<Vector3> _request) 
        {
            ThreadStart _threadStart = () => 
            {
                //instance.pathfinding.FindPath (_request, FinishedProcessingPath);
            };
            
            _threadStart.Invoke();
        }

        public void FinishedProcessingPath(PathResult _result) 
        {
            lock (results) 
            {
                results.Enqueue(_result);
            }
        }
    }
}
