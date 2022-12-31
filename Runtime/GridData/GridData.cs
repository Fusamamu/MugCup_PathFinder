using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class GridData<T>: MonoBehaviour where T : INode
    {
        public T[]        GridNodes;
        public Vector3Int GridSize ;
        
        public GridData<T> InitializeGridUnitSize(Vector3Int _gridSize)
        {
            GridSize = _gridSize;
            return this;
        }
        
        public GridData<T> InitializeGridArray()
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
        
        public T GetNode(Vector3Int _nodePos)
        {
            return GridUtility.GetNode(_nodePos, GridSize, GridNodes);
        }
        
        public void AddNode(T _newNode, Vector3Int _nodePos)
        {
            GridUtility.AddNode(_newNode, _nodePos, GridSize, ref GridNodes);
        }
		
        public void RemoveNode(Vector3Int _nodePos)
        {
            GridUtility.RemoveNode(_nodePos, GridSize, ref GridNodes); //Need ref?? Probably not
        }

        public virtual void ClearData()
        {
            GridNodes = null;
        }
        
        // public void EmptyGridUnitNodeBases()
        // {
        //     for (var _i = 0; _i < GridUnitNodes.Length; _i++)
        //     {
        //         GridUnitNodes[_i] = null;
        //     }
        // }
        //
        // public void ClearGridUnitNodeBases()
        // {
        //     GridUnitNodes = null;
        // }
    }
}
