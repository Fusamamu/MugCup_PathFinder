using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathFinderControllerGeneric<T> : MonoBehaviour, IPathFinderController<T> where T :NodeBase, IHeapItem<T>
    {
        private readonly Queue<PathResult<T>> pathResults = new Queue<PathResult<T>>();

        private HeapPathFinderGeneric<T> pathFinder;
        
        [SerializeField] private GridNodeData<T> gridNodeData;

        private bool isInit;
       
        public virtual void Initialized(GridNodeData<T> _gridNodeData)
        {
            if(isInit) return;
            isInit = true;
                
            InjectGridDataNode  (_gridNodeData);
            InitializePathFinder();
        }
      
        protected void InjectGridDataNode(GridNodeData<T> _gridNodeData)
        {
            gridNodeData = _gridNodeData;
        }

        private void InitializePathFinder()
        {
            pathFinder = new HeapPathFinderGeneric<T>(gridNodeData.GridSize, gridNodeData.GridNodes);
        }

        private void Update() 
        {
            if (pathResults.Count > 0) 
            {
                int _itemsInQueue = pathResults.Count;
                
                for(var _i = 0; _i < _itemsInQueue; _i++) 
                {
                    PathResult<T> _result = pathResults.Dequeue();
                    
                    _result.OnPathFound(_result.Path, _result.Success);
                }
            }
        }

        public void RequestPath(PathRequest<T> _request)
        {
            Task _findPathTask = Task.Run(() =>
            {
                pathFinder.FindPath(_request, FinishedProcessingPath);
            });
        }

        private void FinishedProcessingPath(PathResult<T> _result) 
        {
            pathResults.Enqueue(_result);
            Debug.Log("Path Process Completed. Path's result enqueued.");
        }
        
        //Test Request Path
        // public void RequestPath()
        // {
        //     var _startNode  = GridUtility.GetNode(new Vector3Int(0, 0, 0), gridNodeData.GridSize, gridNodeData.GridNodes);
        //     var _targetNode = GridUtility.GetNode(new Vector3Int(4, 0, 4), gridNodeData.GridSize, gridNodeData.GridNodes);
        //
        //     var _pathRequest = new PathRequestNodeBase(_startNode, _targetNode, ((_bases, _b) => { } ));
        //     
        //     RequestPath(_pathRequest);
        // }
    }
}

