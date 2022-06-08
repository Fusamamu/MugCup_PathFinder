using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathFinderController : MonoBehaviour
    {
        private readonly Queue<PathResultVec3> results = new Queue<PathResultVec3>();

        private PathRequest<Vector3Int> currentPathRequest;

        private bool isProcessingPath;

        private void Awake()
        {
            //Need PathFinder Component//
        }

        private void Update() 
        {
            if (results.Count > 0) 
            {
                int _itemsInQueue = results.Count;
                
                lock (results) 
                {
                    for(var _i = 0; _i < _itemsInQueue; _i++) 
                    {
                        PathResultVec3 _result = results.Dequeue();
                        
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

        public void FinishedProcessingPath(PathResultVec3 _result) 
        {
            lock (results) 
            {
                results.Enqueue(_result);
            }
        }
    }
}
