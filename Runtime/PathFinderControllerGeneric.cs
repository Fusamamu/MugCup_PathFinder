using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathFinderControllerGeneric<T> : MonoBehaviour where T :NodeBase, IHeapItem<T>
    {
        private readonly Queue<PathResult<T>> pathResults = new Queue<PathResult<T>>();

        private HeapPathFinderGeneric<T> pathFinder;
        
        [SerializeField] private GridNodeData<T> gridNodeData;

        private bool isInit;

        public void Initialized()
        {
            if(isInit) return;
            isInit = true;
                
            InjectCustomGridNodeData(gridNodeData);
            InitializePathFinder();
        }
      
        public void InjectCustomGridNodeData(GridNodeData<T> _gridNodeData)
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

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;
//
// namespace MugCup_PathFinder.Runtime
// {
//     //Need to make this Singleton?
//     public class PathFinderController<T> : MonoBehaviour where T : NodeBase, IHeapItem<T>
//     {
//         private readonly Queue<PathResult<T>> results = new Queue<PathResult<T>>();
//
//         private bool isProcessingPath;
//
//         private HeapPathFinder pathFinder;
//         
//         [SerializeField] private bool useGridNodeDataManager;
//         
//         [SerializeField] private GridNodeDataManager    gridNodeDataManager;
//         [SerializeField] private GridNodeData<NodeBase> gridNodeData;
//
//         private bool isInit;
//
//         public void SetUseGridNodeDataManager(bool _value)
//         {
//             useGridNodeDataManager = _value;
//         }
//
//         public void Initialized()
//         {
//             if(isInit) return;
//             isInit = true;
//             
//             if(useGridNodeDataManager)
//                 InjectGridNodeData();
//             else
//                 InjectCustomGridNodeData(gridNodeData);
//             
//             InitializePathFinder();
//         }
//
//         /// <summary>
//         /// Inject GridNodeData into PathFinderController. 
//         /// </summary>
//         /// <param name="_gridNodeDataManager"></param>
//         private void InjectGridNodeData(GridNodeDataManager _gridNodeDataManager = null)
//         {
//             /*Using GridNodeData from GridNodeDataManager this is out of the box data from Path Finder Package*/
//             gridNodeDataManager = _gridNodeDataManager != null ? _gridNodeDataManager : FindObjectOfType<GridNodeDataManager>();
//
//             if (!gridNodeDataManager)
//             {
//                 Debug.LogWarning($"GridNodeData Missing Reference.");
//                 return;
//             }
//
//             gridNodeData.GridSize  = gridNodeDataManager.GetGridSize ();
//             gridNodeData.GridNodes = gridNodeDataManager.GetGridNodes();
//         }
//
//         /// <summary>
//         /// Inject Custom GridNodeData if not using GridNodeDataManager from PathFinder Package.
//         /// </summary>
//         /// <param name="_gridNodeData"></param>
//         public void InjectCustomGridNodeData(GridNodeData<NodeBase> _gridNodeData)
//         {
//             //Use grid node data from scene (from Block Builder)
//             gridNodeData = _gridNodeData;
//         }
//
//         private void InitializePathFinder()
//         {
//             pathFinder = new HeapPathFinder(gridNodeData.GridSize, gridNodeData.GridNodes);
//         }
//
//         private void Update() 
//         {
//             if (results.Count > 0) 
//             {
//                 int _itemsInQueue = results.Count;
//                 
//                 for(var _i = 0; _i < _itemsInQueue; _i++) 
//                 {
//                     PathResult<T> _result = results.Dequeue();
//                     
//                     _result.OnPathFound(_result.Path, _result.Success);
//                 }
//             }
//         }
//
//         //Test Request Path
//         public void RequestPath()
//         {
//             var _startNode  = GridUtility.GetNode(new Vector3Int(0, 0, 0), gridNodeData.GridSize, gridNodeData.GridNodes);
//             var _targetNode = GridUtility.GetNode(new Vector3Int(4, 0, 4), gridNodeData.GridSize, gridNodeData.GridNodes);
//
//             var _pathRequest = new PathRequest<T>(_startNode, _targetNode, ((_bases, _b) => { } ));
//             
//             RequestPath(_pathRequest);
//         }
//
//         // public void RequestPath(PathRequestNodeBase _request)
//         // {
//         //     Task _findPathTask = Task.Run(() =>
//         //     {
//         //         pathFinder.FindPath(_request, FinishedProcessingPaths);
//         //     });
//         // }
//         //
//         // private void FinishedProcessingPaths(PathResultNodeBase _result) 
//         // {
//         //     results.Enqueue(_result);
//         //     Debug.Log("Path Process Completed. Path's result enqueued.");
//         // }
//
//         public void RequestPath(PathRequest<T> _request)
//         {
//             Task _findPathTask = Task.Run(() =>
//             {
//                 pathFinder.FindPath<T>(_request, FinishedProcessingPath);
//             });
//         }
//         
//         private void FinishedProcessingPath(PathResult<T> _result)
//         {
//             results.Enqueue(_result);
//             
//             Debug.Log("Path Process Completed. Path's result enqueued.");
//         }
//
//     }
// }

