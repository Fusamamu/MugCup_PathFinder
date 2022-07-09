using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
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
}
