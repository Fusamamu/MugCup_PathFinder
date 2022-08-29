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
       
        /// <summary>
        /// Select Target GridDataNode Used to Calculate Path.
        /// Must be called before Initialized.
        /// </summary>
        /// <param name="_gridNodeData"></param>
        /// <returns></returns>
        public IPathFinderController<T> SelectGridDataNode(GridNodeData<T> _gridNodeData)
        {
            gridNodeData = _gridNodeData;
            return this;
        }
        
        /// <summary>
        /// Must be called Initialized.
        /// </summary>
        /// <returns></returns>
        public IPathFinderController<T> InitializePathFinder()
        {
            pathFinder = new HeapPathFinderGeneric<T>
            (
                gridNodeData.GridSize, 
                gridNodeData.GridNodes
            );
            return this;
        }
        
        public virtual void Initialized()
        {
            if(isInit) return;
            isInit = true;
        }

        private void Update() 
        {
            if (pathResults.Count > 0) 
            {
                var _itemsInQueue = pathResults.Count;
                
                for(var _i = 0; _i < _itemsInQueue; _i++) 
                {
                    PathResult<T> _result = pathResults.Dequeue();
                    
                    _result.OnPathFound(_result.Path, _result.Success);
                }
            }
        }

        public void RequestPath(PathRequest<T> _request, bool _waitForComplete = false)
        {
            Task _findPathTask = Task.Run(() =>
            {
                pathFinder.FindPath(_request, FinishedProcessingPath);
            });

            if (_waitForComplete)
                _findPathTask.Wait();
        }

        private void FinishedProcessingPath(PathResult<T> _result) 
        {
            pathResults.Enqueue(_result);
            Debug.Log("Path Process Completed. Path's result enqueued.");
        }
    }
}

