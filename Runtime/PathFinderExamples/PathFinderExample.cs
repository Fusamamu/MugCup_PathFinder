using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MugCup_PathFinder.Runtime.Examples
{
    public class TestNode : INode
    {
        public INode NodeParent { get; set; }
        
        public Vector3Int NodePosition      { get; set; }
        public Vector3    NodeWorldPosition { get; set;  }
        
        public Vector3Int NextNodePosition  { get; set; }
        public Vector3    ExitPosition      { get; set; }
        
        public int G_Cost { get; set; }
        public int H_Cost { get; set; }
        public int F_Cost => G_Cost + H_Cost;

        public HashSet<INode> Neighbors { get; }
        
        public INode NorthNode { get; }
        public INode SouthNode { get; }
        public INode WestNode  { get; }
        public INode EastNode  { get; }
        
        public INode NextNodeOnPath { get; set; }
    
        public NodeDirection Direction { get; set; }
        
        public void SetNextNodeOnPath(INode _node)
        {
		    
        }
        public void SetNodePathDirection(NodeDirection _direction)
        {
		    
        }
    
        public INode GrowPathTo(INode _neighbor, NodeDirection _direction)
        {
            return default;
        }
    }

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

        private IEnumerable<GridNode> GetPathUsingHeapPathFinder()
        {
            pathFinder = new HeapPathFinder(gridSize, gridNodes);
            var _path = pathFinder.FindPath(gridNodes[0], gridNodes[40]).ToArray();

            return _path;
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
