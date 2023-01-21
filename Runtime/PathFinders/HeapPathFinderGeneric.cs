// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
//
// namespace MugCup_PathFinder.Runtime
// {
// 	/// <summary>
//     /// Using Heap sorting to find the node which least cost.
//     /// </summary>
//     public class HeapPathFinderGeneric<T> : IPathFinder<T> where T : GridNode, IHeapItem<T>
//     {
//         public Vector3Int GridSize  { get; private set; }  
//         public T[]        GridNodes { get; private set; }
//         
//         private readonly int maxIteration;
//
//         public HeapPathFinderGeneric(Vector3Int _gridSize, T[] _nodes, int _maxIteration = 50)
//         {
//             GridSize     = _gridSize;
//             GridNodes    = _nodes;
//             maxIteration = _maxIteration;
//         }
//         
//         public IEnumerable<T> FindPath(T _origin, T _target)
//         {
//             Heap   <T> _openSet   = new Heap   <T>(GridSize.x * GridSize.z);
//             HashSet<T> _closedSet = new HashSet<T>();
//             
//             _openSet.Add(_origin);
//
//             int _iter = 0;
//             while (_openSet.Count > 0 && _iter < maxIteration)
//             {
//                 _iter++;
//                 
//                 T _node = _openSet.RemoveFirst();
//                 
//                 _closedSet.Add(_node);
//
//                 if (_node == _target)
//                     return AStarPathFinder<T>.RetracePath(_origin, _target);
//
//                 List<T> _adjacentNodes = GridUtility.GetAdjacentNodes8Dir(_node, GridSize, GridNodes).ToList();
//
//                 foreach (T _adjacent in _adjacentNodes)
//                 {
//                     if(_closedSet.Contains(_adjacent) || _adjacent == null)
//                         continue;
//                     
//                     int _newCostToAdjacent = _node.G_Cost + AStarPathFinder<INode>.GetDistance(_node, _adjacent);
//
//                     if (_newCostToAdjacent < _adjacent.G_Cost || !_openSet.Contains(_adjacent))
//                     {
//                         _adjacent.G_Cost = _newCostToAdjacent;
//                         _adjacent.H_Cost = AStarPathFinder<INode>.GetDistance(_adjacent, _target);
//                         _adjacent.NodeParent = _node;
//                         
//                         if(!_openSet.Contains(_adjacent))
//                             _openSet.Add(_adjacent);
//                         else
//                             _openSet.UpdateItem(_adjacent);
//                     }
//                 }
//             }
//             
//             return null;
//         }
//         
//         public void FindPath(PathRequest<T> _pathRequest, Action<PathResult<T>> _onPathFound)
//         {
//             T[] _waypoints = Array.Empty<T>();
//             
//             bool _pathFound = false;
//
//             T _startNode  = _pathRequest.PathStart as T;
//             T _targetNode = _pathRequest.PathEnd   as T;
//
//             if (_startNode == null)
//             {
//                 Debug.Log("StartNode not found.");
//                 return;
//             }
//             
//             _startNode.NodeParent = _startNode;
//             
//             //if (startNode.walkable && targetNode.walkable) 
//             
//             Heap   <T> _openSet   = new Heap   <T>(GridSize.x * GridSize.z);
//             HashSet<T> _closedSet = new HashSet<T>();
//             
//             _openSet.Add(_startNode);
//
//             int _iter = 0;
//             
//             while (_openSet.Count > 0 && _iter < maxIteration)
//             {
//                 _iter++;
//                 
//                 T _currentNode = _openSet.RemoveFirst();
//                 
//                 _closedSet.Add(_currentNode);
//
//                 if (_currentNode == _targetNode)
//                 {
//                     _pathFound = true;
//                     break;
//                 }
//                 
//                 //List<T> _adjacentNodes = GridUtility.GetAdjacentNodes8Dir(_currentNode, GridSize, GridNodes).ToList();
//                 List<T> _adjacentNodes = GridUtility.GetAdjacentNodes4Dir(_currentNode, GridSize, GridNodes).ToList();
//
//                 foreach (T _adjacent in _adjacentNodes)
//                 {
//                     // if (!neighbour.walkable || closedSet.Contains(neighbour)) {
//                     //     continue;
//                     // }
//                     
//                     if(_closedSet.Contains(_adjacent) || _adjacent == null)
//                         continue;
//                     
//                     int _newCostToAdjacent = _currentNode.G_Cost + AStarPathFinder<INode>.GetDistance(_currentNode, _adjacent);
//
//                     if (_newCostToAdjacent < _adjacent.G_Cost || !_openSet.Contains(_adjacent))
//                     {
//                         _adjacent.G_Cost     = _newCostToAdjacent;
//                         _adjacent.H_Cost     = AStarPathFinder<INode>.GetDistance(_adjacent, _targetNode);
//                         _adjacent.NodeParent = _currentNode;
//                         
//                         if(!_openSet.Contains(_adjacent))
//                             _openSet.Add(_adjacent);
//                         else
//                             _openSet.UpdateItem(_adjacent);
//                     }
//                 }
//             }
//             
//             if (_pathFound)
//             {
//                 _waypoints = AStarPathFinder<T>.RetracePath(_startNode, _targetNode).ToArray();
//                 
//                 AStarPathFinder<T>.SetNodePathData(_waypoints.ToList());
//
//                 _pathFound = _waypoints.Length > 0;
//             }
//
//             var _pathResult = new PathResult<T>(_waypoints, _pathFound, _pathRequest.Callback);
//             
//             _onPathFound(_pathResult);
//         }
//         
//         public IEnumerable<Vector3Int> FindPath(Vector3Int _origin, Vector3Int _target)
//         {
//             return null;
//         }
//     }
// }
