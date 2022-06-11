using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
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
        
        [SerializeField] private GridNodeData gridNodeData;

        //Should replace with singleton GridNodeData    
        //[SerializeField] private GridNodeDataSetting gridNodeDataSetting;
       
        
        //Need to have something hold "Grid Size" n "GridNode" Data
        private Vector3Int gridSize;
        private NodeBase[] gridNodes;
        
        
        
        private void Start()
        {
            InjectGridNodeData  ();
            InitializePathFinder();
        }

        /// <summary>
        /// Inject GridNodeData into PathFinderController. 
        /// </summary>
        /// <param name="_gridNodeData"></param>
        private void InjectGridNodeData(GridNodeData _gridNodeData = null)
        {
            gridNodeData= _gridNodeData != null ? _gridNodeData : FindObjectOfType<GridNodeData>();

            if (!gridNodeData)
            {
                Debug.LogWarning($"GridNodeData Missing Reference.");
                return;
            }

            gridSize     = gridNodeData.GetGridSize ();
            gridNodes    = gridNodeData.GetGridNodes();
        }

        private void InitializePathFinder()
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

        //Test Request Path
        public void RequestPath()
        {
            var _startNode  = GridUtility.GetNode(new Vector3Int(0, 0, 0), gridSize, gridNodes);
            var _targetNode = GridUtility.GetNode(new Vector3Int(4, 0, 4), gridSize, gridNodes);

            var _pathRequest = new PathRequestNodeBase(_startNode, _targetNode, ((_bases, _b) => { } ));
            
            RequestPath(_pathRequest);
        }

        public void RequestPath(PathRequestNodeBase _request)
        {
            Task _findPathTask = Task.Run(() =>
            {
                pathFinder.FindPath(_request, FinishedProcessingPath);
            });
            //_findPathTask.Wait();
        }

        private void FinishedProcessingPath(PathResultNodeBase _result) 
        {
            results.Enqueue(_result);
            Debug.Log("Path Process Completed. Path's result enqueued.");
        }
    }
}
