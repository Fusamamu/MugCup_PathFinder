using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IPathFinder<T> where T : INode
    {
        public Vector3Int GridSize  { get; } 
        public T[]        GridNodes { get; } 
        
        // public Vector3Int GetGridSize ();
        // public T[]        GetGridNodes();

        public IEnumerable<T>          FindPath(T _origin, T _target);
        public IEnumerable<Vector3Int> FindPath(Vector3Int _origin, Vector3Int _target);
    }

    /// <summary>
    /// Using simple loop iteration to find the node with least cost.
    /// </summary>
    public class SimplePathFinder : IPathFinder<INode> 
    {
        public Vector3Int GridSize  { get; set; }
        public INode[]    GridNodes { get; set; }

        public Vector3Int GetGridSize()
        {
            return GridSize;
        }
        public INode[] GetGridNodes()
        {
            return GridNodes;
        }

        private readonly int maxIteration;

        public SimplePathFinder(Vector3Int _gridSize, INode[] _nodes, int _maxIteration = 50)
        {
            GridSize     = _gridSize;
            GridNodes    = _nodes;
            maxIteration = _maxIteration;
        }
        
        public IEnumerable<INode> FindPath(INode _origin, INode _target)
        {
            List<INode>    _openSet   = new List<INode>();
            HashSet<INode> _closedSet = new HashSet<INode>();
            
            _openSet.Add(_origin);

            int _iter = 0;
            while (_openSet.Count > 0 && _iter < maxIteration)
            {
                _iter++;
                    
                INode _node = AStarPathFinder<INode>.GetNodeLeastCost(_openSet);
                _openSet.Remove(_node);
                
                _closedSet.Add(_node);

                if (_node == _target)
                    return AStarPathFinder<INode>.RetracePath(_origin, _target);

                List<INode> _adjacentNodes = GridUtility.GetAdjacentNodes8Dir(_node, GridSize, GridNodes).ToList();

                foreach (INode _adjacent in _adjacentNodes)
                {
                    if(_closedSet.Contains(_adjacent) || _adjacent == null)
                        continue;
                    
                    int _newCostToAdjacent = _node.G_Cost + AStarPathFinder<INode>.GetDistance(_node, _adjacent);

                    if (_newCostToAdjacent < _adjacent.G_Cost || !_openSet.Contains(_adjacent))
                    {
                        _adjacent.G_Cost = _newCostToAdjacent;
                        _adjacent.H_Cost = AStarPathFinder<INode>.GetDistance(_adjacent, _target);
                        _adjacent.NodeParent = _node;
                        
                        if(!_openSet.Contains(_adjacent))
                            _openSet.Add(_adjacent);
                    }
                }
            }
            
            return null;
        }

        public IEnumerable<Vector3Int> FindPath(Vector3Int _origin, Vector3Int _target)
        {
            return null;
        }
    }

    /// <summary>
    /// Global static class uses to calculate A*Star Path on the fly.
    /// It can also be used as method helpers related to Path Finder.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class AStarPathFinder<T> where T: class, INode
    {
        public static Vector3Int GridSize;
        public static T[] Nodes;
            
        public static T Origin;
        public static T Target;

        public static void InitializeGridData(Vector3Int _gridSize, T[] _nodes)
        {
            GridSize = _gridSize;
            Nodes    = _nodes;
        }

        public static IEnumerable<T> FindPath(T _origin, T _target)
        {
            List<T>    _openSet   = new List<T>();
            HashSet<T> _closedSet = new HashSet<T>();
            
            _openSet.Add(_origin);

            int _iter = 0;
            while (_openSet.Count > 0 && _iter < 50)
            {
                _iter++;
                    
                T _node = GetNodeLeastCost(_openSet);
                _openSet.Remove(_node);
                
                _closedSet.Add(_node);

                if (_node == _target)
                    return RetracePath(_origin, _target);

                List<T> _adjacentNodes = GridUtility.GetAdjacentNodes8Dir(_node, GridSize, Nodes).ToList();

                foreach (T _adjacent in _adjacentNodes)
                {
                    if(_closedSet.Contains(_adjacent) || _adjacent == null)
                        continue;
                    
                    int _newCostToAdjacent = _node.G_Cost + GetDistance(_node, _adjacent);

                    if (_newCostToAdjacent < _adjacent.G_Cost || !_openSet.Contains(_adjacent))
                    {
                        _adjacent.G_Cost = _newCostToAdjacent;
                        _adjacent.H_Cost = GetDistance(_adjacent, _target);
                        _adjacent.NodeParent = _node;
                        
                        if(!_openSet.Contains(_adjacent))
                            _openSet.Add(_adjacent);
                    }
                }
            }
            
            return null;
        }
        
        public static T GetNodeLeastCost(List<T> _openSet)
        {
            T _node = _openSet[0];

            for (var _i = 1; _i < _openSet.Count; _i++)
            {
                if (_openSet[_i].F_Cost <= _node.F_Cost)
                {
                    if (_openSet[_i].H_Cost < _node.H_Cost)
                        _node = _openSet[_i];
                }
            }
            return _node;
        }

        public static IEnumerable<T> RetracePath(T _origin, T _target)
        {
            List<T> _path = new List<T>();
            
            T _currentNode = _target;

            while (_currentNode != _origin)
            {
                _path.Add(_currentNode);
                _currentNode = _currentNode?.NodeParent as T;
            }
            
            _path.Reverse();
            
            return _path;
        }

        public static void SetNodePathData(List<T> _path)
        {
            for (var _i = 0; _i < _path.Count - 1; _i++)
            {
                var _currentNode = _path[_i];
                var _nextNode    = _path[_i + 1];
                
                _currentNode.SetNextNodeOnPath(_nextNode);

                var _currentPos = _currentNode.NodePosition;
                var _nextPos    = _nextNode   .NodePosition;

                _currentNode.NextNodePosition = _nextPos;
                
                NodeDirection _direction = NodeDirection.None;

                if (_nextPos.x > _currentPos.x)
                {
                    _direction = NodeDirection.East;
                }
                else if (_nextPos.x < _currentPos.x)
                {
                    _direction = NodeDirection.West;
                }
                else if (_nextPos.z > _currentPos.z)
                {
                    _direction = NodeDirection.North;
                }
                else if (_nextPos.z < _currentPos.z)
                {
                    _direction = NodeDirection.South;
                }
                
                _currentNode.SetNodePathDirection(_direction);
            }
        }
        
        public static int GetDistance(T _nodeA, T _nodeB)
        {
            int _distanceX = Math.Abs(_nodeA.NodePosition.x - _nodeB.NodePosition.x);
            int _distanceY = Math.Abs(_nodeA.NodePosition.y - _nodeB.NodePosition.y);

            if (_distanceX > _distanceY)
                return 14 * _distanceY + 10 * (_distanceX - _distanceY);

            return 14 * _distanceX + 10 * (_distanceY - _distanceX);
        }
        
        public static Vector3[] SimplifyPath(List<T> _path) 
        {
            var _waypoints    = new List<Vector3>();
            var _oldDirection = Vector2.zero;
		
            for (var _i = 1; _i < _path.Count; _i++) 
            {
                // Vector2 _newDirection = new Vector2(_path[_i-1].gridX - _path[_i].gridX,_path[_i-1].gridY - _path[_i].gridY);
                //
                // if (_newDirection != _oldDirection) 
                // {
                //     _waypoints.Add(_path[_i].worldPosition);
                // }
                
                //_oldDirection = _newDirection;
            }
            
            return _waypoints.ToArray();
        }
    }
}
