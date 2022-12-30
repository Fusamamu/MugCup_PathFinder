using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    [Serializable]
    public class GridVertexData : GridNodeData<VertexNode>
    {
        
    }

    [Serializable]
    public class GridNodeBaseData : GridNodeData<NodeBase>
    {
        
    }

    [Serializable]
    public class GridNodeData<T> where T : INode
    {
        public T[]        GridNodes;
        public Vector3Int GridSize ;
        
        public GridNodeData<T> InitializeGridUnitSize(Vector3Int _gridSize)
        {
            GridSize = _gridSize;
            return this;
        }
        
        public GridNodeData<T> InitializeGridArray()
        { 
            int _rowUnit    = GridSize.x;
            int _columnUnit = GridSize.z;
            int _levelUnit  = GridSize.y;

            GridNodes = new T[_rowUnit * _columnUnit * _levelUnit];

            for (int _y = 0; _y < _levelUnit ; _y++)
            for (int _x = 0; _x < _rowUnit   ; _x++)
            for (int _z = 0; _z < _columnUnit; _z++)
                GridNodes[_z + GridSize.x * (_x + GridSize.y * _y)] = default(T);

            return this;
        }
    }
}
