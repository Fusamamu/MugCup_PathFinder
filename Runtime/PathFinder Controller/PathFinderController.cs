using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathFinderController<T> : MonoBehaviour, IPathFinderController<T> where T :GridNode, IHeapItem<T>
    {
        private readonly Queue<PathResult<T>> pathResults = new Queue<PathResult<T>>();

        private HeapPathFinderGeneric<T> pathFinder;
        
        [SerializeField] private GridData<T> GridData;

        private bool isInit;

        /// <summary>
        /// Select Target GridDataNode Used to Calculate Path.
        /// Must be called before Initialized.
        /// </summary>
        /// <param name="_gridData"></param>
        /// <returns></returns>
        public IPathFinderController<T> SelectGridDataNode(GridData<T> _gridData)
        {
            GridData = _gridData;
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
                GridData.GridSize, 
                GridData.GridNodes
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
        
        public async Task RequestPathAsync(PathRequest<T> _request, Action _onCompleted)
        {
            await Task.Run(() =>
            {
                pathFinder.FindPath(_request, FinishedProcessingPath);
            });
            
            _onCompleted?.Invoke();
        }

        private void FinishedProcessingPath(PathResult<T> _result) 
        {
            pathResults.Enqueue(_result);
            Debug.Log("Path Process Completed. Path's result enqueued.");
        }
    }
}

