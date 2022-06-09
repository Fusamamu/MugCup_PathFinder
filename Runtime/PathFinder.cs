using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MugCup_PathFinder.Runtime.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MugCup_PathFinder.Runtime
{
    //Todo
    //Path Finder Methods
    //1. By code using class AStarPathFinder
    //2. By Attach PathFinder Component to each agent
    //3. By PathFinder Controller
    
    /// <summary>
    /// This is a Path Finder Component, used to custom your own properties
    /// </summary>
    public class PathFinder : MonoBehaviour, IPathFinder<NodeBase>
    {
        public NodeBase[] PathNodes => pathNodes;
        
        public Vector3Int GetGridSize()
        {
            return gridSize;
        }
        public NodeBase[] GetGridNodes()
        {
            return gridNodes;
        }
        
        public bool HasPath => pathNodes.Length > 1;

        //1. Use Agent as Start Node
        //2. If no Agent, Set Start Node and Target Node Manually.
        //3. Use Raycast in Editor to get Start and Target Node.
        
        //Might need to move to Custom Editor Class
        public enum TargetType
        {
            UseAgent, SetManual, UseRaycast
        }

        public TargetType StartPathType = TargetType.SetManual;
        public TargetType EndPathType   = TargetType.SetManual;
        //------------------------------------//
        
        [SerializeField] private Agent agent; //If Assigned use agent pos instead of start posiiton
        
        [SerializeField] private Vector3Int gridSize;
        
        [SerializeField] private Vector3Int startPosition;
        [SerializeField] private Vector3Int targetPosition;
        
        [SerializeField] private NodeBase startNode;
        [SerializeField] private NodeBase targetNode;

        [SerializeField] private NodeBase[] gridNodes;
        [SerializeField] private NodeBase[] pathNodes;
        
        private IPathFinder<NodeBase> pathFinder;

        [SerializeField] private int maxIteration = 50;
        
        [SerializeField] private float nodeRadius = 0.025f;

        private void OnValidate()
        {
            if (startPosition.x >= gridSize.x)
                startPosition.x = gridSize.x - 1;

            if (startPosition.x < 0)
                startPosition.x = 0;

            if (startPosition.z >= gridSize.z)
                startPosition.z = gridSize.z - 1;

            if (startPosition.z < 0)
                startPosition.z = 0;
        }

        private void Start()
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
            ClearPath();
            
            gridNodes = GridUtility.GenerateGridINodes<NodeBase>(gridSize);
            
            var _pathFinder = new HeapPathFinder(gridSize, gridNodes, maxIteration);

            NodeBase _startNode  = null;
            NodeBase _targetNode = null;

            foreach (var _node in gridNodes)
            {
                if (_node.NodePosition == startPosition) //Need to implement IComparable
                    _startNode = _node;

                if (_node.NodePosition == targetPosition)
                    _targetNode = _node;
            }

            if (_startNode == null)
            {
                Debug.Log("Start Node null");
                return null;
            }
            if (_targetNode == null)
            {
                Debug.Log("Target Node null");
                return null;
            }
            
            pathNodes = _pathFinder.FindPath(_startNode, _targetNode).ToArray();

            return pathNodes;
        }

        public void ClearPath()
        {
            foreach (var _node in gridNodes)
                DestroyImmediate(_node.gameObject);
            
            gridNodes = Array.Empty<NodeBase>();
            pathNodes = Array.Empty<NodeBase>();
        }

        private IEnumerable<NodeBase> GetPath(Vector3Int _gridSize, NodeBase[] _gridNodes)
        {
            pathFinder = new HeapPathFinder(_gridSize, _gridNodes, maxIteration);
            
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

        public IEnumerable<NodeBase> FindPath(NodeBase _origin, NodeBase _target)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<Vector3Int> FindPath(Vector3Int _origin, Vector3Int _target)
        {
            throw new NotImplementedException();
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
        }
        
    }
}
