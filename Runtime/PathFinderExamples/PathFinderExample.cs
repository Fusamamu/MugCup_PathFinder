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
        
        public int G_Cost { get; set; }
        public int H_Cost { get; set; }
        public int F_Cost => G_Cost + H_Cost;
    }

    public class PathFinderExample : MonoBehaviour
    {
        [SerializeField] private NodeBase startNode;
        [SerializeField] private NodeBase targetNode;
        
        [SerializeField] private Vector3Int gridSize;

        [SerializeField] private NodeBase[] gridNodes;

        private IPathFinder<NodeBase> pathFinder;

        void Start()
        {
            var _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gridNodes = GridUtility.GenerateGridINodes<NodeBase>(gridSize, _cube);

            var _path = GetPathUsingHeapPathFinder().ToArray();

            var _pathGizmos = FindObjectOfType<PathGizmos>();
            
            _pathGizmos.SetGridRef(_path);
        }

        private IEnumerable<NodeBase> GetPathUsingHeapPathFinder()
        {
            pathFinder = new HeapPathFinder(gridSize, gridNodes);
            var _path = pathFinder.FindPath(gridNodes[0], gridNodes[40]).ToArray();

            return _path;
        }

        private IEnumerable<NodeBase> GetPathUsingStaticPathFinder()
        {
            AStarPathFinder<NodeBase>.InitializeGridData(gridSize, gridNodes);
            
            var _path = AStarPathFinder<NodeBase>.FindPath(gridNodes[0], gridNodes[40]).ToArray();

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
