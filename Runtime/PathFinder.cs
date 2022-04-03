using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MugCup_PathFinder.Runtime.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MugCup_PathFinder.Runtime
{
    /// <summary>
    /// This is a Path Finder Controller, used to custom your own properties
    /// </summary>
    public class PathFinder : MonoBehaviour
    {
        public Vector3Int GridSize => gridSize;
        
        [SerializeField] private Vector3Int gridSize;
        
        [SerializeField] private Vector3Int startPosition;
        [SerializeField] private Vector3Int targetPosition;
        
        [SerializeField] private NodeBase startNode;
        [SerializeField] private NodeBase targetNode;

        [SerializeField] private NodeBase[] gridNodes;
        [SerializeField] private NodeBase[] pathNodes;
        
        private IPathFinder<NodeBase> pathFinder;
        
        [SerializeField] private float nodeRadius = 0.025f;
            
        [SerializeField] private Color nodeColor = Color.blue;

        void Start()
        {
            gridSize = new Vector3Int(10, 1, 10);
            
            var _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gridNodes = GridUtility.GenerateGridINodes<NodeBase>(gridSize, _cube);

            var _path = GetPath(gridSize, gridNodes).ToArray();
            
            _path.ToList().ForEach(_p =>
            {
                _p.NodePosition += new Vector3Int(0, 1, 0);
            });

            var _pathGizmos = gameObject.AddComponent<PathGizmos>();
            
            _pathGizmos.SetGridRef(_path);
        }

        public IEnumerable<NodeBase> GeneratePath()
        {
            gridNodes = GridUtility.GenerateGridINodes<NodeBase>(gridSize);
            
            var _pathFinder = new HeapPathFinder(gridSize, gridNodes);

            NodeBase _startNode  = null;
            NodeBase _targetNOde = null;

            foreach (var _node in gridNodes)
            {
                if (_node.NodePosition == startPosition)
                    _startNode = _node;

                if (_node.NodePosition == targetPosition)
                    _targetNOde = _node;
            }

            pathNodes = _pathFinder.FindPath(_startNode, _targetNOde).ToArray();

            return pathNodes;
        }

        public void Clear()
        {
            foreach (var _node in gridNodes)
                DestroyImmediate(_node.gameObject);
            
            Array.Clear(gridNodes, 0, gridNodes.Length);
            Array.Clear(pathNodes, 0, pathNodes.Length);
        }

        private IEnumerable<NodeBase> GetPath(Vector3Int _gridSize, NodeBase[] _gridNodes)
        {
            pathFinder = new HeapPathFinder(_gridSize, _gridNodes);
            
            var _path = pathFinder.FindPath(_gridNodes[0], _gridNodes[40]).ToArray();

            return _path;
        }

        public IEnumerable<NodeBase> GetPath(Vector3Int _startPos, Vector3Int _targetPos)
        {
            return null;
        }

        public IEnumerable<NodeBase> GetPath(NodeBase _startNode, NodeBase _targetNode)
        {
            return null;
        }
        
        

        void Update()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            
            if(gridNodes == null || gridNodes.Length == 0) return;
            
            
            foreach (var _node in gridNodes)
            {
                Gizmos.DrawSphere(_node.NodePosition, nodeRadius);
            }
            
            DrawGridGizmos(new Vector3Int(10, 1, 10));
        }

        private void DrawGridGizmos(Vector3Int _gridSize)
        {
            Gizmos.color = Color.green;

            Vector3 _p0 = Vector3.zero;
            Vector3 _p1 = Vector3.zero;

            for (var _i = 0; _i < _gridSize.x; _i++)
            {
                _p0 = new Vector3(_i,     0, 0);
                _p1 = new Vector3(_i + 1, 0, 0);
                
                Gizmos.DrawLine(_p0, _p1);
            }
        }
    }
}
