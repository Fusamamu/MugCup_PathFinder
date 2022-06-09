using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    //Need to make this Singleton?
    public class PathFinderController : MonoBehaviour
    {
        private readonly Queue<PathResultNodeBase> results = new Queue<PathResultNodeBase>();

        private PathResultNodeBase currentPathRequest;

        private bool isProcessingPath;

        private HeapPathFinder pathFinder;

        [SerializeField] private bool useGridNodeData;

        //This should replace gridSize/gridNodes below?
        //Or we're able to switch between local and global data.
        [SerializeField] private GridNodeData gridNodeData;
        
        //Need to have something hold "Grid Size" n "GridNode" Data
        [SerializeField] private Vector3Int gridSize;
        [SerializeField] private NodeBase[] gridNodes;

        private void Awake()
        {
            pathFinder = new HeapPathFinder(gridSize, gridNodes);
        }

        private void Update() 
        {
            if (results.Count > 0) 
            {
                int _itemsInQueue = results.Count;
                   
                for(var _i = 0; _i < _itemsInQueue; _i++) 
                {
                    PathResultNodeBase _result = results.Dequeue();
                        
                    _result.OnPathFound(_result.Path, _result.Success);
                }
            }
        }

        public void RequestPath()
        {
            
        }

        public void RequestPath(PathRequestNodeBase _request)
        {
            pathFinder.FindPath(_request, FinishedProcessingPath);
        }

        private void FinishedProcessingPath(PathResultNodeBase _result) 
        {
            results.Enqueue(_result);
        }
    }
}
