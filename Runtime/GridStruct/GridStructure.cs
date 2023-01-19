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
        public float3 WorldOffset;

        public int3 GridSize;
        
        public int Row;
        public int Column;
        public int Level;
        
        public float NodeSize;

        public NativeArray<NodeNativeData> Grid; 
        
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
        
        // public PathfindingGrid Copy (Allocator allocator = Allocator.TempJob) {
        //     PathfindingGrid newGrid = this;
        //     newGrid.grid = new NativeArray<Node>(grid.Length, allocator);
        //     grid.CopyTo(newGrid.grid);
        //     return newGrid;
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
