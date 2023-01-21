using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace MugCup_PathFinder.Runtime
{
    public static class GridStructureBuilder
    {
        public static GridStructure GenerateGridStructure(Vector3Int _gridSize)
        {
            var _gridStructure = new GridStructure
            {
                WorldOffset = float3.zero,
                
                GridSize =  new int3(_gridSize.x, _gridSize.y, _gridSize.z),
                
                Row     =  _gridSize.x,
                Column  =  _gridSize.z,
                Level   =  _gridSize.y,
                
                Grid = new NativeArray<NodeNativeData>(_gridSize.x * _gridSize.y * _gridSize.z, Allocator.Persistent)
            };

            return _gridStructure;
        }

        public static GridStructure CopyData<T>(this GridData<T> _gridData) where T : INode
        {
            var _gridStructure = GenerateGridStructure(_gridData.GridSize);
            
            for(var _i = 0; _i < _gridData.GridNodes.Length; _i++)
            {
                var _node = _gridData.GridNodes[_i];
                
                if(_node == null) continue;
                
                var _newNode = new NodeNativeData
                {
                    GridPos  = new int3  (_node.NodeGridPosition.x , _node.NodeGridPosition.y , _node.NodeGridPosition.z),
                    WorldPos = new float3(_node.NodeWorldPosition.x, _node.NodeWorldPosition.y, _node.NodeWorldPosition.z)
                };

                _gridStructure.Grid[_i] = _newNode;
            }

            return _gridStructure;
        }
    }
}
