using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    //Broken
    
    /// <summary>
    /// Using simple loop iteration to find the node with least cost.
    /// </summary>
    public class SimplePathFinder : IPathFinder<INode> 
    {
        public Vector3Int GridSize  { get; set; }
        public INode[]    GridNodes { get; set; }
        
        public void FindPath(PathRequest<INode> _pathRequest, Action<PathResult<INode>> _onPathFound)
        {
            throw new NotImplementedException();
        }

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
        
        //New interface
        public GridData<INode> GridData {
            get;
        }
        public void FindPath(PathRequest<Vector3Int> _pathRequest, Action<PathResult<Vector3Int>> _onPathFound)
        {
            throw new NotImplementedException();
        }
    }
}
