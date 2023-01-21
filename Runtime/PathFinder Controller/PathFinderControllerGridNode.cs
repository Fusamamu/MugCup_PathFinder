using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathFinderControllerGridNode : MonoBehaviour, IPathFinderController<GridNode>
    {
        private bool isInit;
        
        [SerializeField] private GridData<GridNode> GridData;
        
        private IPathFinder<GridNode> pathFinder;
        
        private readonly Queue<PathResult<Vector3Int>> pathResults = new Queue<PathResult<Vector3Int>>();

        public IPathFinderController<GridNode> SetGridData(GridData<GridNode> _gridData)
        {
            GridData = _gridData;
            return this;
        }
        
        public IPathFinderController<GridNode> SetPathFinder()
        {
            pathFinder = new HeapPathFinder(GridData);
            return this;
        }
        
        public void Initialized()
        {
            if(isInit) return;
            isInit = true;
        }
        
        public void RequestPath(PathRequest<Vector3Int> _request, bool _waitForComplete)
        {
            Task _findPathTask = Task.Run(() =>
            {
                pathFinder.FindPath(_request, FinishedProcessingPath);
            });

            if (_waitForComplete)
                _findPathTask.Wait();
        }

        private void FinishedProcessingPath(PathResult<Vector3Int> _result) 
        {
            pathResults.Enqueue(_result);
            
            Debug.Log("Path Process Completed. Path's result enqueued.");
        }
        
        private void Update() 
        {
            if (pathResults.Count > 0) 
            {
                var _itemsInQueue = pathResults.Count;
                
                for(var _i = 0; _i < _itemsInQueue; _i++) 
                {
                    PathResult<Vector3Int> _result = pathResults.Dequeue();
                    
                    _result.OnPathFound(_result.Path, _result.Success);
                }
            }
        }
    }
}

