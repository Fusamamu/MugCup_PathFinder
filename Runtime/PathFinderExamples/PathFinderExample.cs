using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MugCup_PathFinder.Runtime.Examples
{
    //Broken need to update to new interface
    public class PathFinderExample : MonoBehaviour
    {
        [SerializeField] private GridNode StartGridNode;
        [SerializeField] private GridNode TargetGridNode;
        
        [SerializeField] private Vector3Int gridSize;

        [SerializeField] private GridNode[] gridNodes;

        private IPathFinder<GridNode> pathFinder;

        void Start()
        {
            var _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gridNodes = GridUtility.GenerateGridINodes<GridNode>(gridSize, _cube);

            var _path = GetPathUsingHeapPathFinder().ToArray();

            var _pathGizmos = FindObjectOfType<PathGizmos>();
            
            _pathGizmos.SetGridRef(_path);
        }

        //Broken need update to new interface
        private IEnumerable<GridNode> GetPathUsingHeapPathFinder()
        {
            //pathFinder = new HeapPathFinder(gridSize, gridNodes);
            //var _path = pathFinder.FindPath(gridNodes[0], gridNodes[40]).ToArray();

            return null;
        }

        private IEnumerable<GridNode> GetPathUsingStaticPathFinder()
        {
            AStarPathFinder<GridNode>.InitializeGridData(gridSize, gridNodes);
            
            var _path = AStarPathFinder<GridNode>.FindPath(gridNodes[0], gridNodes[40]).ToArray();

            return _path;
        }

        void Update()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                
            }
        }
    }
}
