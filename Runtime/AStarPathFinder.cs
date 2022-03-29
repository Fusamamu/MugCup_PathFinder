using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IPathFinder<T> where T : INode
    {
        public Vector3Int GridSize { get; set; }
        public T[] Nodes           { get; set; }
        
        public IEnumerable<T> FindPath(T _origin, T _target);
    }

    public class SimplePathFinder : IPathFinder<INode> 
    {
        public Vector3Int GridSize { get; set; }
        public INode[] Nodes       { get; set; }

        private readonly int maxIteration;

        public SimplePathFinder(Vector3Int _gridSize, INode[] _nodes, int _maxIteration = 50)
        {
            GridSize = _gridSize;
            Nodes    = _nodes;

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

                List<INode> _adjacentNodes = GridUtility.GetAdjacentNodes8Dir(_node, GridSize, Nodes).ToList();

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
    }

    public class HeapPathFinder : IPathFinder<NodeBase>
    {
        public Vector3Int GridSize { get; set; }  
        public NodeBase[] Nodes    { get; set; }
        
        private readonly int maxIteration;

        public HeapPathFinder(Vector3Int _gridSize, NodeBase[] _nodes, int _maxIteration = 50)
        {
            GridSize = _gridSize;
            Nodes    = _nodes;
            
            maxIteration = _maxIteration;
        }
        
        public IEnumerable<NodeBase> FindPath(NodeBase _origin, NodeBase _target)
        {
            Heap<NodeBase> _openSet      = new Heap<NodeBase>(GridSize.x * GridSize.z);
            HashSet<NodeBase> _closedSet = new HashSet<NodeBase>();
            
            _openSet.Add(_origin);

            int _iter = 0;
            while (_openSet.Count > 0 && _iter < maxIteration)
            {
                _iter++;
                
                NodeBase _node = _openSet.RemoveFirst();
                
                _closedSet.Add(_node);

                if (_node == _target)
                    return AStarPathFinder<NodeBase>.RetracePath(_origin, _target);

                List<NodeBase> _adjacentNodes = GridUtility.GetAdjacentNodes8Dir(_node, GridSize, Nodes).ToList();

                foreach (NodeBase _adjacent in _adjacentNodes)
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

            for (int i = 1; i < _openSet.Count; i++)
            {
                if (_openSet[i].F_Cost <= _node.F_Cost)
                {
                    if (_openSet[i].H_Cost < _node.H_Cost)
                        _node = _openSet[i];
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
        
        public static int GetDistance(T _nodeA, T _nodeB)
        {
            int _distanceX = Math.Abs(_nodeA.NodePosition.x - _nodeB.NodePosition.x);
            int _distanceY = Math.Abs(_nodeA.NodePosition.y - _nodeB.NodePosition.y);

            if (_distanceX > _distanceY)
                return 14 * _distanceY + 10 * (_distanceX - _distanceY);

            return 14 * _distanceX + 10 * (_distanceY - _distanceX);
        }
    }
}
