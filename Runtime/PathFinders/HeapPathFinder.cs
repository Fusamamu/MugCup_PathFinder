using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class HeapPathFinder: IPathFinder<GridNode>
    {
        public GridData<GridNode> GridData { get; private set; }
        
        private readonly int maxIteration;

        public HeapPathFinder(GridData<GridNode> _gridData, int _maxIteration = 50)
        {
            GridData     = _gridData;
            maxIteration = _maxIteration;
        }
     
        public void FindPath(PathRequest<Vector3Int> _pathRequest, Action<PathResult<Vector3Int>> _onPathFound)
        {
            GridNode[] _nodePath = Array.Empty<GridNode>();
            
            bool _pathFound = false;

            GridNode _startGridNode  = GridData.GetNode(_pathRequest.PathStart);
            GridNode _targetGridNode = GridData.GetNode(_pathRequest.PathEnd);
            
            if (_startGridNode == null)
            {
                Debug.Log("StartNode not found.");
                return;
            }
            
            _startGridNode.NodeParent = _startGridNode;
            
            //if (startNode.walkable && targetNode.walkable) 
            
            Heap   <GridNode> _openSet   = new Heap   <GridNode>(GridData.GridSize.x * GridData.GridSize.z);
            HashSet<GridNode> _closedSet = new HashSet<GridNode>();
            
            _openSet.Add(_startGridNode);

            int _iter = 0;
            
            while (_openSet.Count > 0 && _iter < maxIteration)
            {
                _iter++;
                
                GridNode _currentGridNode = _openSet.RemoveFirst();
                
                _closedSet.Add(_currentGridNode);

                if (_currentGridNode == _targetGridNode)
                {
                    _pathFound = true;
                    break;
                }

                List<GridNode> _adjacentNodes = GridUtility.GetAdjacentNodes4Dir(_currentGridNode, GridData.GridSize, GridData.GridNodes).ToList();

                foreach (GridNode _adjacent in _adjacentNodes)
                {
                    // if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    //     continue;
                    // }
                    
                    if(_closedSet.Contains(_adjacent) || _adjacent == null)
                        continue;
                    
                    int _newCostToAdjacent = _currentGridNode.G_Cost + AStarPathFinder<INode>.GetDistance(_currentGridNode, _adjacent);

                    if (_newCostToAdjacent < _adjacent.G_Cost || !_openSet.Contains(_adjacent))
                    {
                        _adjacent.G_Cost     = _newCostToAdjacent;
                        _adjacent.H_Cost     = AStarPathFinder<INode>.GetDistance(_adjacent, _targetGridNode);
                        _adjacent.NodeParent = _currentGridNode;
                        
                        if(!_openSet.Contains(_adjacent))
                            _openSet.Add(_adjacent);
                        else
                            _openSet.UpdateItem(_adjacent);
                    }
                }
            }
            
            if (_pathFound)
            {
                _nodePath = AStarPathFinder<GridNode>.RetracePath(_startGridNode, _targetGridNode).ToArray();
                
                AStarPathFinder<GridNode>.SetNodePathData(_nodePath.ToList());
                
                _pathFound = _nodePath.Length > 0;
            }

            var _wayPoints = _nodePath.ToList().Select(_node => _node.NodeGridPosition).ToArray();

            var _pathResult = new PathResult<Vector3Int>(_wayPoints, _pathFound, _pathRequest.Callback);
            
            _onPathFound(_pathResult);
        }
    }
}
