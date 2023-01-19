using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public static class GridDataBuilder 
    {
        public static T[] InitializeGridArray<T>(Vector3Int _gridSize) where T : INode
        { 
            int _rowUnit    = _gridSize.x;
            int _columnUnit = _gridSize.z;
            int _levelUnit  = _gridSize.y;
            
            var _gridNodes = new T[_rowUnit * _columnUnit * _levelUnit];
            
            for (var _y = 0; _y < _levelUnit ; _y++)
            for (var _x = 0; _x < _rowUnit   ; _x++)
            for (var _z = 0; _z < _columnUnit; _z++)
                _gridNodes[_z + _gridSize.x * (_x + _gridSize.y * _y)] = default(T);

            return _gridNodes;
        }
        
        public static T[] GenerateGridNodes<T>(Vector3Int _gridUnitSize, GameObject _nodePrefab, GameObject _parent = null) where T : INode
        {
            int _rowUnit    = _gridUnitSize.x;
            int _columnUnit = _gridUnitSize.z;
            int _levelUnit  = _gridUnitSize.y;
            
            T[] _nodes = new T[_rowUnit * _columnUnit * _levelUnit];
            
            for (int _y = 0; _y < _levelUnit; _y++)
            {
                for (int _x = 0; _x < _rowUnit; _x++)
                {
                    for (int _z = 0; _z < _columnUnit; _z++)
                    {
                        var _position = new Vector3(_x, _y, _z);

                        var _nodeObject = Object.Instantiate(_nodePrefab, _position, Quaternion.identity);

                        if (_parent != null)
                        {
                            _nodeObject.transform.position += _parent.transform.position;
                            _nodeObject.transform.SetParent(_parent.transform);
                        }

                        if (_nodeObject.TryGetComponent<T>(out var _node))
                        {
                            _node
                                .SetNodePosition     (new Vector3Int(_x, _y, _z))
                                .SetNodeWorldPosition(_nodeObject.transform.position);
                            
                            _nodes[_z + _gridUnitSize.x * (_x + _gridUnitSize.y * _y)] = _node;
                        }
                    }
                }
            }

            return _nodes;
        }
        
        public static void GenerateGridAtLevel<T>(int _level) where T : INode
        {
            // var _blockPrefab  = AssetManager.AssetCollection.DefualtBlock.gameObject;
            //
            // GridBlockGenerator.PopulateGridBlocksByLevel<Block>(GridUnitNodes, GridUnitSize, _level, _blockPrefab);
            //
            // var _selectedBlockLevel = GetAllNodeBasesAtLevel<GridNode>(_level);
            //
            // GridNodeData.GridNodes = GridUnitNodes;
            // GridNodeData.GridSize  = GridUnitSize;
            //
            // if(!levelTable.ContainsKey(_level))
            //     levelTable.Add(_level, _selectedBlockLevel);
            // else
            //     levelTable[_level] = _selectedBlockLevel;
        }
    }
}
