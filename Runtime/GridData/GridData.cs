using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class GridData<T>: MonoBehaviour where T : IGridCoord
    {
        public IEnumerable<T> ValidNodes 
        {
            get
            {
                foreach (var _node in GridNodes)
                {
                    if (_node != null)
                        yield return _node;
                }
            }
        }

        public T[]        GridNodes;
        public Vector3Int GridSize ;
        
        public int RowUnit   ;
        public int ColumnUnit;
        public int LevelUnit ;

        public GridData<T> SetGridSize(Vector3Int _gridSize)
        {
            GridSize = _gridSize;
            return this;
        }

        public GridData<T> InitializeGridUnitSize(Vector3Int _gridSize)
        {
            GridSize = _gridSize;
            return this;
        }

        public virtual void Initialized()
        {
            throw new NotImplementedException();
        }
        
        //Duplicate Code
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

        public TU[] GetGridUnitArray<TU>() where TU : GridNode
        {
            var _gridUnitArray = new TU[GridNodes.Length];

            for (var _i = 0; _i < _gridUnitArray.Length; _i++)
            {
                _gridUnitArray[_i] = GridNodes[_i] as TU;
            }

            return _gridUnitArray;
        }
        
        public IEnumerable<TU> AvailableNodes<TU>() where TU : class, INode
        {
            foreach (var _node in GridNodes)
            {
                if(_node == null) continue;
        
                if (_node is TU _block)
                    yield return _block;
            }
        }

        public void ApplyAllNode(Action<T> _action)
        {
            foreach (var _node in ValidNodes)
            {
                _action?.Invoke(_node);
            }
        }
        
        public void ApplyAllNodes<TU>(Action<TU> _action) where TU : class, INode
        {
            foreach (var _node in ValidNodes)
            {
                if(_node is TU _castNode)
                    _action?.Invoke(_castNode);
            }
        }

        public bool TryGetNode(Vector3Int _nodePos, out T _node)
        {
            _node = GetNode(_nodePos);
            return _node != null;
        }
        
        public T GetNode(Vector3Int _nodePos)
        {
            return GridUtility.GetNode(_nodePos, GridSize, GridNodes);
        }
        
        public TU GetNode<TU>(Vector3Int _nodePos) where TU : class, INode
        {
            return GridUtility.GetNode(_nodePos, GridSize, GridNodes) as TU;
        }
        
#region Get Node Via Direction
        public T GetNodeForward(Vector3Int _nodePos)
        {
            return GridUtility.GetNodeForward(_nodePos, GridSize, GridNodes);
        }

        public T GetNodeRight(Vector3Int _nodePos)
        {
            return GridUtility.GetNodeRight(_nodePos, GridSize, GridNodes);
        }

        public T GetNodeBack(Vector3Int _nodePos)
        {
            return GridUtility.GetNodeBack(_nodePos, GridSize, GridNodes);
        }

        public T GetNodeLeft(Vector3Int _nodePos)
        {
            return GridUtility.GetNodeLeft(_nodePos, GridSize, GridNodes);
        }
        
        public T GetNodeUp(Vector3Int _nodePos)
        {
            return GridUtility.GetNodeUp(_nodePos, GridSize, GridNodes);
        }

        public T GetNodeDown(Vector3Int _nodePos)
        {
            return GridUtility.GetNodeDown(_nodePos, GridSize, GridNodes);
        }
#endregion
        
        public void AddNode(T _newNode, Vector3Int _nodePos)
        {
            GridUtility.AddNode(_newNode, _nodePos, GridSize, ref GridNodes);
        }
        
        public void AddNode<TU>(TU _newNode, Vector3Int _nodePos) where TU : class, INode
        {
            //GridUtility.AddNode(_newNode, _nodePos, GridSize, ref GridNodes);
        }
		
        public void RemoveNode(Vector3Int _nodePos)
        {
            GridUtility.RemoveNode(_nodePos, GridSize, ref GridNodes); //Need ref?? Probably not
        }
        
        public void RemoveNode<TU>(Vector3Int _nodePos) where TU : class, INode
        {
            GridUtility.RemoveNode(_nodePos, GridSize, ref GridNodes);
        }

        public T[] GetAllNodeBasesAtLevel(int _gridLevel)
        {
            int _rowUnit    = GridSize.x;
            int _columnUnit = GridSize.z;
            int _heightUnit = GridSize.y;
            
            var _nodesAtLevel = new T[_rowUnit * _columnUnit];
            
            for (var _x = 0; _x < _rowUnit   ; _x++)
            for (var _z = 0; _z < _columnUnit; _z++)
                _nodesAtLevel[_z + _rowUnit * _x] = GridNodes[_z + _rowUnit * (_x + _heightUnit * _gridLevel)];

            return _nodesAtLevel;
        }
        
        public TU[] GetAllNodeBasesAtLevel<TU>(int _gridLevel) where TU : class, INode
        {
            int _rowUnit    = GridSize.x;
            int _columnUnit = GridSize.z;
            int _heightUnit = GridSize.y;
            
            var _nodesAtLevel = new TU[_rowUnit * _columnUnit];
            
            for (var _x = 0; _x < _rowUnit   ; _x++)
            for (var _z = 0; _z < _columnUnit; _z++)
                _nodesAtLevel[_z + _rowUnit * _x] = GridNodes[_z + _rowUnit * (_x + _heightUnit * _gridLevel)] as TU;

            return _nodesAtLevel;
        }
        
        public void EmptyGrid()
        {
            for (var _i = 0; _i < GridNodes.Length; _i++)
            {
                GridNodes[_i] = default;
            }
        }

        public virtual void ClearData()
        {
            GridNodes = null;
        }
    }
}
