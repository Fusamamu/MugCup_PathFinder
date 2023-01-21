using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace MugCup_PathFinder.Runtime
{
    //IGrid??
    public struct GridStructure : IDisposable
    {
        public int NodeCount => Row * Column * Level;
        
        public float NodeSize;
        public float3 WorldOffset;

        public int3 GridSize;
        
        public int Row;
        public int Column;
        public int Level;

        public NativeArray<NodeNativeData> Grid;

        public NodeNativeData GetNode(int3 _nodePos)
        {
            if (!IsInBound(_nodePos)) return default;
            
            int _x = _nodePos.x;
            int _y = _nodePos.y;
            int _z = _nodePos.z;
            
            return Grid[_z + GridSize.x * (_x + GridSize.y * _y)];
        }

        public float3 GetNodeWorldPos(int3 _nodePos)
        {
            return GetNode(_nodePos).WorldPos;
        }

        public bool IsInBound(int3 _nodePos)
        {
            if (_nodePos.x < 0 || _nodePos.x >= GridSize.x) return false;
            if (_nodePos.y < 0 || _nodePos.y >= GridSize.y) return false;
            if (_nodePos.z < 0 || _nodePos.z >= GridSize.z) return false;

            return true;
        }
       
        public GridStructure Copy(Allocator _allocator = Allocator.TempJob) {
            
            GridStructure _newGrid = this;
            
            _newGrid.Grid = new NativeArray<NodeNativeData>(Grid.Length, _allocator);
            
            Grid.CopyTo(_newGrid.Grid);
            
            return _newGrid;
        }
        
        // public float3 GetNodePosition(int x, int y) {
        //     float yPos = grid[y * width + x].yPosition;
        //
        //     return new float3 {
        //         x = worldOffset.x - (nodeSize * width / 2) + (x * nodeSize) + (nodeSize / 2),
        //         y = worldOffset.y + yPos,
        //         z = worldOffset.z - (nodeSize * height / 2) + (y * nodeSize) + (nodeSize / 2)
        //     };
        // }
        //
        // public float3 GetNodePosition (int2 index) {
        //     return GetNodePosition(index.x, index.y);
        // }
        
        // public Node GetNode(int x, int y) {
        //     return grid[y * width + x];
        // }
        
        // public int2 GetNodeIndex(float3 worldPosition) {
        //     float3 localPos = worldPosition - worldOffset;
        //
        //     int rx = (int)((localPos.x / nodeSize) + (width / 2));
        //     int ry = (int)((localPos.z / nodeSize) + (height / 2));
        //
        //     if(rx < 0 || rx >= width || ry < 0 || ry >= height) {
        //         return new int2 { x = -1, y = -1 };
        //     }
        //
        //     return new int2 { x = rx, y = ry };
        // }

        public void Dispose() 
        {
            if (Grid.IsCreated) 
            {
                Grid.Dispose();
            }
        }
    }
}
