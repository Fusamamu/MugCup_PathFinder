using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IPathFinder<T> where T : INode
    {
        // public Vector3Int GridSize  { get; set; }
        // public T[]        GridNodes { get; set; }

        public Vector3Int GetGridSize ();
        public T[]        GetGridNodes();

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
    /// Using Heap sorting to find the node which least cost.
    /// </summary>
    public class HeapPathFinder : IPathFinder<NodeBase>
    {
        public Vector3Int GridSize  { get; set; }  
        public NodeBase[] GridNodes { get; set; }
        
        public Vector3Int GetGridSize()
        {
            return GridSize;
        }
        public NodeBase[] GetGridNodes()
        {
            return GridNodes;
        }
        
        private readonly int maxIteration;

        public HeapPathFinder(Vector3Int _gridSize, NodeBase[] _nodes, int _maxIteration = 50)
        {
            GridSize     = _gridSize;
            GridNodes    = _nodes;
            maxIteration = _maxIteration;
        }
        
        public IEnumerable<NodeBase> FindPath(NodeBase _origin, NodeBase _target)
        {
            Heap   <NodeBase> _openSet   = new Heap   <NodeBase>(GridSize.x * GridSize.z);
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

                List<NodeBase> _adjacentNodes = GridUtility.GetAdjacentNodes8Dir(_node, GridSize, GridNodes).ToList();

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
                        else
                            _openSet.UpdateItem(_adjacent);
                    }
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// Find a path with PathRequest. If Success, return PathResult.
        /// </summary>
        /// <param name="_pathRequest"></param>
        /// <param name="_onPathFound"></param>
        public void FindPath(PathRequestNodeBase _pathRequest, Action<PathResultNodeBase> _onPathFound)
        {
            NodeBase[] _waypoints = Array.Empty<NodeBase>();
            
            bool _pathFound = false;

            NodeBase _startNode  = _pathRequest.PathStart;
            NodeBase _targetNode = _pathRequest.PathEnd;
            
            _startNode.NodeParent = _startNode;
            
            //if (startNode.walkable && targetNode.walkable) 
            
            Heap   <NodeBase> _openSet   = new Heap   <NodeBase>(GridSize.x * GridSize.z);
            HashSet<NodeBase> _closedSet = new HashSet<NodeBase>();
            
            _openSet.Add(_startNode);

            int _iter = 0;
            while (_openSet.Count > 0 && _iter < maxIteration)
            {
                _iter++;
                
                NodeBase _currentNode = _openSet.RemoveFirst();
                
                _closedSet.Add(_currentNode);

                if (_currentNode == _targetNode)
                {
                    _pathFound = true;
                    break;
                }

                List<NodeBase> _adjacentNodes = GridUtility.GetAdjacentNodes8Dir(_currentNode, GridSize, GridNodes).ToList();

                foreach (NodeBase _adjacent in _adjacentNodes)
                {
                    // if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    //     continue;
                    // }
                    
                    if(_closedSet.Contains(_adjacent) || _adjacent == null)
                        continue;
                    
                    int _newCostToAdjacent = _currentNode.G_Cost + AStarPathFinder<INode>.GetDistance(_currentNode, _adjacent);

                    if (_newCostToAdjacent < _adjacent.G_Cost || !_openSet.Contains(_adjacent))
                    {
                        _adjacent.G_Cost     = _newCostToAdjacent;
                        _adjacent.H_Cost     = AStarPathFinder<INode>.GetDistance(_adjacent, _targetNode);
                        _adjacent.NodeParent = _currentNode;
                        
                        if(!_openSet.Contains(_adjacent))
                            _openSet.Add(_adjacent);
                        else
                            _openSet.UpdateItem(_adjacent);
                    }
                }
            }
            
            if (_pathFound)
            {
                _waypoints = AStarPathFinder<NodeBase>.RetracePath(_pathRequest.PathStart, _pathRequest.PathEnd).ToArray();
                _pathFound = _waypoints.Length > 0;
            }

            var _pathResult = new PathResultNodeBase(_waypoints, _pathFound, _pathRequest.Callback);
            
            _onPathFound(_pathResult);
        }
        
        public void FindPath<T>(PathRequest<T> _pathRequest, Action<PathResult<T>> _onPathFound) where T : NodeBase, IHeapItem<T>
        {
            T[] _waypoints = Array.Empty<T>();
            
            bool _pathFound = false;

            T _startNode  = _pathRequest.PathStart as T;
            T _targetNode = _pathRequest.PathEnd   as T;

            if (_startNode == null)
            {
                Debug.Log("StartNode not found.");
                return;
            }
            
            _startNode.NodeParent = _startNode;
            
            //if (startNode.walkable && targetNode.walkable) 
            
            Heap   <T> _openSet   = new Heap   <T>(GridSize.x * GridSize.z);
            HashSet<T> _closedSet = new HashSet<T>();
            
            _openSet.Add(_startNode);

            int _iter = 0;
            
            while (_openSet.Count > 0 && _iter < maxIteration)
            {
                _iter++;
                
                T _currentNode = _openSet.RemoveFirst();
                
                _closedSet.Add(_currentNode);

                if (_currentNode == _targetNode)
                {
                    _pathFound = true;
                    break;
                }

                var _castNodes = Utilities.CovertAllNodes<T>(GridNodes);
                
                List<T> _adjacentNodes = GridUtility.GetAdjacentNodes8Dir<T>(_currentNode, GridSize, _castNodes).ToList();

                foreach (T _adjacent in _adjacentNodes)
                {
                    // if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    //     continue;
                    // }
                    
                    if(_closedSet.Contains(_adjacent) || _adjacent == null)
                        continue;
                    
                    int _newCostToAdjacent = _currentNode.G_Cost + AStarPathFinder<INode>.GetDistance(_currentNode, _adjacent);

                    if (_newCostToAdjacent < _adjacent.G_Cost || !_openSet.Contains(_adjacent))
                    {
                        _adjacent.G_Cost     = _newCostToAdjacent;
                        _adjacent.H_Cost     = AStarPathFinder<INode>.GetDistance(_adjacent, _targetNode);
                        _adjacent.NodeParent = _currentNode;
                        
                        if(!_openSet.Contains(_adjacent))
                            _openSet.Add(_adjacent);
                        else
                            _openSet.UpdateItem(_adjacent);
                    }
                }
            }
            
            if (_pathFound)
            {
                _waypoints = AStarPathFinder<T>.RetracePath(_startNode, _targetNode).ToArray();
                _pathFound = _waypoints.Length > 0;
            }

            var _pathResult = new PathResult<T>(_waypoints, _pathFound, _pathRequest.Callback);
            
            _onPathFound(_pathResult);
        }
        
        public IEnumerable<Vector3Int> FindPath(Vector3Int _origin, Vector3Int _target)
        {
            return null;
        }

        private Vector3Int CastVec3ToVec3Int(Vector3 _value)
        {
            return new Vector3Int((int)_value.x, (int)_value.y, (int)_value.z);
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
