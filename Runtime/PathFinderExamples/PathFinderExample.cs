using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

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
        [SerializeField] private Vector3Int gridSize;

        [SerializeField] private NodeBase[] grid;
        
        void Start()
        {
            var _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            grid = GridUtility.GenerateGridINodes<NodeBase>(gridSize, _cube);
            
            AStar<NodeBase>.InitializeGridData(gridSize, grid);
        }

        void Update()
        {
            
        }
    }
}
