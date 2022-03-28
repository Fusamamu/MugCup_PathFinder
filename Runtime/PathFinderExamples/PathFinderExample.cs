using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MugCup_PathFinder.Runtime.Utilities;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

namespace MugCup_PathFinder.Runtime.Examples
{
    public class TestNode : INode
    {
        public INode NodeParent { get; set; }
        
        public Vector3Int NodePosition { get; set; }
        
        public int G_Cost { get; set; }
        public int H_Cost { get; set; }
        public int F_Cost => G_Cost + H_Cost;
    }

    public class PathFinderExample : MonoBehaviour
    {
        [SerializeField] private NodeBase startNode;
        [SerializeField] private NodeBase targetNode;
        
        [SerializeField] private Vector3Int gridSize;

        [SerializeField] private NodeBase[] grid;
        
        void Start()
        {
            var _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            grid = GridUtility.GenerateGridINodes<NodeBase>(gridSize, _cube);
            
            AStarPathFinder<NodeBase>.InitializeGridData(gridSize, grid);

            var _path = AStarPathFinder<NodeBase>.FindPath(grid[0], grid[40]).ToArray();



            var _pathGizmos = FindObjectOfType<PathGizmos>();
            
            _pathGizmos.SetGridRef(_path);
        }

        void Update()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                
            }
        }
    }
}
