using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MugCup_PathFinder.Runtime
{
    public static class GridUtility
    {
        public static IEnumerable<T> GetAdjacentNodes4Dir<T>(T _node, Vector3Int _gridSize, T[] _grid) where T: INode
        {
            List<T> _adjacentBlocks = new List<T>
            {
                GetNodeLeft        (_node.NodePosition, _gridSize, _grid),
                GetNodeRight       (_node.NodePosition, _gridSize, _grid),
                GetNodeForward     (_node.NodePosition, _gridSize, _grid),
                GetNodeBack        (_node.NodePosition, _gridSize, _grid)
            };

            return _adjacentBlocks;
        }
        
        public static IEnumerable<T> GetAdjacentNodes8Dir<T>(T _node, Vector3Int _gridSize, T[] _grid) where T: INode
        {
            List<T> _adjacentBlocks = new List<T>
            {
                GetNodeLeft        (_node.NodePosition, _gridSize, _grid),
                GetNodeRight       (_node.NodePosition, _gridSize, _grid),
                GetNodeForward     (_node.NodePosition, _gridSize, _grid),
                GetNodeBack        (_node.NodePosition, _gridSize, _grid),
                GetNodeForwardLeft (_node.NodePosition, _gridSize, _grid),
                GetNodeForwardRight(_node.NodePosition, _gridSize, _grid),
                GetNodeBackLeft    (_node.NodePosition, _gridSize, _grid),
                GetNodeBackRight   (_node.NodePosition, _gridSize, _grid)
            };

            return _adjacentBlocks;
        }

#region GetNodes From 3x3 Cube [by node reference]
        public static IEnumerable<T> GetNodesFrom3x3Cubes<T>(T _node, Vector3Int _gridSize, T[] _grid) where T : INode
        {
            List<T> _cubeNodes = new List<T>();
            
            _cubeNodes.AddRange(GetTopSectionNodesFrom3x3Cube   (_node, _gridSize, _grid).ToList());
            _cubeNodes.AddRange(GetMiddleSectionNodesFrom3x3Cube(_node, _gridSize, _grid).ToList());
            _cubeNodes.AddRange(GetBottomSectionNodesFrom3x3Cube(_node, _gridSize, _grid).ToList());
            
            return _cubeNodes;
        }
        
        public static IEnumerable<T> GetTopSectionNodesFrom3x3Cube<T>(T _node, Vector3Int _gridSize, T[] _grid) where T: INode
        {
            List<T> _topSectionNodes = new List<T>
            {
                GetNodeUpBackLeft    (_node.NodePosition, _gridSize, _grid),
                GetNodeUpLeft        (_node.NodePosition, _gridSize, _grid),
                GetNodeUpForwardLeft (_node.NodePosition, _gridSize, _grid),
                GetNodeUpBack        (_node.NodePosition, _gridSize, _grid),
                GetNodeUp            (_node.NodePosition, _gridSize, _grid),
                GetNodeUpForward     (_node.NodePosition, _gridSize, _grid),
                GetNodeUpBackRight   (_node.NodePosition, _gridSize, _grid),
                GetNodeUpRight       (_node.NodePosition, _gridSize, _grid),
                GetNodeUpForwardRight(_node.NodePosition, _gridSize, _grid)
            };

            return _topSectionNodes;
        }
        
        public static IEnumerable<T> GetMiddleSectionNodesFrom3x3Cube<T>(T _node, Vector3Int _gridSize, T[] _grid) where T: INode
        {
            List<T> _middleSectionNodes = new List<T>
            {
                GetNodeBackLeft    (_node.NodePosition, _gridSize, _grid),
                GetNodeLeft        (_node.NodePosition, _gridSize, _grid),
                GetNodeForwardLeft (_node.NodePosition, _gridSize, _grid),
                GetNodeBack        (_node.NodePosition, _gridSize, _grid),
                GetNode            (_node.NodePosition, _gridSize, _grid),
                GetNodeForward     (_node.NodePosition, _gridSize, _grid),
                GetNodeBackRight   (_node.NodePosition, _gridSize, _grid),
                GetNodeRight       (_node.NodePosition, _gridSize, _grid),
                GetNodeForwardRight(_node.NodePosition, _gridSize, _grid)
            };

            return _middleSectionNodes;
        }
        
        public static IEnumerable<T> GetBottomSectionNodesFrom3x3Cube<T>(T _node, Vector3Int _gridSize, T[] _grid) where T: INode
        {
            List<T> _bottomSectionNodes = new List<T>
            {
                GetNodeDownBackLeft    (_node.NodePosition, _gridSize, _grid),
                GetNodeDownLeft        (_node.NodePosition, _gridSize, _grid),
                GetNodeDownForwardLeft (_node.NodePosition, _gridSize, _grid),
                GetNodeDownBack        (_node.NodePosition, _gridSize, _grid),
                GetNodeDown            (_node.NodePosition, _gridSize, _grid),
                GetNodeDownForward     (_node.NodePosition, _gridSize, _grid),
                GetNodeDownBackRight   (_node.NodePosition, _gridSize, _grid),
                GetNodeDownRight       (_node.NodePosition, _gridSize, _grid),
                GetNodeDownForwardRight(_node.NodePosition, _gridSize, _grid)
            };

            return _bottomSectionNodes;
        }
#endregion
        
#region GetNodes From 3x3 Cube [by node position]
        public static IEnumerable<T> GetNodesFrom3x3Cubes<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            List<T> _cubeNodes = new List<T>();
            
            _cubeNodes.AddRange(GetTopSectionNodesFrom3x3Cube   (_nodePos, _gridSize, _grid).ToList());
            _cubeNodes.AddRange(GetMiddleSectionNodesFrom3x3Cube(_nodePos, _gridSize, _grid).ToList());
            _cubeNodes.AddRange(GetBottomSectionNodesFrom3x3Cube(_nodePos, _gridSize, _grid).ToList());
            
            return _cubeNodes;
        }
        
        public static IEnumerable<T> GetTopSectionNodesFrom3x3Cube<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            List<T> _topSectionNodes = new List<T>
            {
                GetNodeUpBackLeft    (_nodePos, _gridSize, _grid),
                GetNodeUpLeft        (_nodePos, _gridSize, _grid),
                GetNodeUpForwardLeft (_nodePos, _gridSize, _grid),
                GetNodeUpBack        (_nodePos, _gridSize, _grid),
                GetNodeUp            (_nodePos, _gridSize, _grid),
                GetNodeUpForward     (_nodePos, _gridSize, _grid),
                GetNodeUpBackRight   (_nodePos, _gridSize, _grid),
                GetNodeUpRight       (_nodePos, _gridSize, _grid),
                GetNodeUpForwardRight(_nodePos, _gridSize, _grid)
            };

            return _topSectionNodes;
        }
        
        public static IEnumerable<T> GetMiddleSectionNodesFrom3x3Cube<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            List<T> _middleSectionNodes = new List<T>
            {
                GetNodeBackLeft    (_nodePos, _gridSize, _grid),
                GetNodeLeft        (_nodePos, _gridSize, _grid),
                GetNodeForwardLeft (_nodePos, _gridSize, _grid),
                GetNodeBack        (_nodePos, _gridSize, _grid),
                GetNode            (_nodePos, _gridSize, _grid),
                GetNodeForward     (_nodePos, _gridSize, _grid),
                GetNodeBackRight   (_nodePos, _gridSize, _grid),
                GetNodeRight       (_nodePos, _gridSize, _grid),
                GetNodeForwardRight(_nodePos, _gridSize, _grid)
            };

            return _middleSectionNodes;
        }
        
        public static IEnumerable<T> GetBottomSectionNodesFrom3x3Cube<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            List<T> _bottomSectionNodes = new List<T>
            {
                GetNodeDownBackLeft    (_nodePos, _gridSize, _grid),
                GetNodeDownLeft        (_nodePos, _gridSize, _grid),
                GetNodeDownForwardLeft (_nodePos, _gridSize, _grid),
                GetNodeDownBack        (_nodePos, _gridSize, _grid),
                GetNodeDown            (_nodePos, _gridSize, _grid),
                GetNodeDownForward     (_nodePos, _gridSize, _grid),
                GetNodeDownBackRight   (_nodePos, _gridSize, _grid),
                GetNodeDownRight       (_nodePos, _gridSize, _grid),
                GetNodeDownForwardRight(_nodePos, _gridSize, _grid)
            };

            return _bottomSectionNodes;
        }
#endregion
        
#region Request Nodes Method;
        public static List<T> GetNodesRectArea<T>(Vector3Int _startCorner, Vector3Int _endCorner, Vector3Int _gridSize,  T[] _grid)
        {
            if (!NodePositionInsideGrid(_startCorner, _gridSize)) return default;

            List<T> _nodes = new List<T>();

            BoundsInt _bounds = GetAABBBounds(_startCorner, _endCorner);

            foreach (Vector3Int _pos in _bounds.allPositionsWithin)
            {
                T _node = GetNode(_pos, _gridSize, _grid);
                _nodes.Add(_node);
            }
            
            return _nodes;
        }
        
        public static BoundsInt GetAABBBounds(Vector3Int _p1, Vector3Int _p2)
        {
            return new BoundsInt(
                Math.Min(_p1.x, _p2.x),
                Math.Min(_p1.y, _p2.y),
                Math.Min(_p1.z, _p2.z),
                Math.Abs(_p2.x - _p1.x) + 1,
                Math.Abs(_p2.y - _p1.y) + 1,
                Math.Abs(_p2.z - _p1.z) + 1
            );
        }
        
        public static T GetNode<T>(Vector3Int _nodePos, GridNodeData<T> _gridNodeData) where T : NodeBase
        {
            var _gridSize = _gridNodeData.GridSize;
            var _grid     = _gridNodeData.GridNodes;

            return GetNode<T>(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNode<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            if (!NodePositionInsideGrid(_nodePos, _gridSize)) return default;
            
            int _x = _nodePos.x;
            int _y = _nodePos.y;
            int _z = _nodePos.z;
            
            T _node = _grid[_z + _gridSize.x * (_x + _gridSize.y * _y)];

            return _node;
        }

        public static bool NodePositionInsideGrid(Vector3Int _nodePos, Vector3Int _gridSize)
        {
            if (_nodePos.x < 0 || _nodePos.x >= _gridSize.x) return false;
            if (_nodePos.y < 0 || _nodePos.y >= _gridSize.y) return false;
            if (_nodePos.z < 0 || _nodePos.z >= _gridSize.z) return false;

            return true;
        }
#endregion

#region Get Node Left, Right, Forward, Back
        public static T GetNodeLeft<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            _nodePos += Vector3Int.left;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeRight<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            _nodePos += Vector3Int.right;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeForward<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            _nodePos += Vector3Int.forward;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeBack<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            _nodePos += Vector3Int.back;
            return GetNode(_nodePos, _gridSize, _grid);
        }
#endregion

#region Get Node ForwardLeft, ForwardRight, BackLeft, BackRight
        public static T GetNodeForwardLeft<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.left;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeForwardRight<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.right;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeBackLeft<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.left;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeBackRight<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.right;
            return GetNode(_nodePos, _gridSize, _grid);
        }
#endregion

#region Get Node Up, UpLeft, UpRight, UpForward, UpBack
        public static T GetNodeUp<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            _nodePos += Vector3Int.up;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeUpLeft<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.left;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeUpRight<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.right;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeUpForward<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.forward;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeUpBack<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid) 
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.back;
            return GetNode(_nodePos, _gridSize, _grid);
        }
#endregion

#region GetNode UpForwardLeft, UpForwardRight, UpBackLeft, UpBackRight
        public static T GetNodeUpForwardLeft<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.left;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeUpForwardRight<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.right;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeUpBackLeft<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.left;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeUpBackRight<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.right;
            return GetNode(_nodePos, _gridSize, _grid);
        }
#endregion
        
#region GetNode Down, DownLeft, DownRight, DownForward, DownBack
        public static T GetNodeDown<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.down;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeDownLeft<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.left;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeDownRight<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.right;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeDownForward<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.forward;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeDownBack<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.back;
            return GetNode(_nodePos, _gridSize, _grid);
        }
#endregion
        
#region GetNode DownForwardLeft, DownForwardRight, DownBackLeft, DownBackRight
        public static T GetNodeDownForwardLeft<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.left;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeDownForwardRight<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.right;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeDownBackLeft<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.left;
            return GetNode(_nodePos, _gridSize, _grid);
        }
        
        public static T GetNodeDownBackRight<T>(Vector3Int _nodePos, Vector3Int _gridSize, T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.right;
            return GetNode(_nodePos, _gridSize, _grid);
        }
#endregion
        
#region Add Node Left, Right, Forward, Back
        public static void AddNodeLeft<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.left;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeRight<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid) 
        {
            _nodePos += Vector3Int.right;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeForward<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid) 
        {
            _nodePos += Vector3Int.forward;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeBack<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid) 
        {
            _nodePos += Vector3Int.back;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
#endregion
        
#region Add Node ForwardLeft, ForwardRight, BackLeft, BackRight
        public static void AddNodeForwardLeft<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid) 
        {
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.left;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeForwardRight<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.right;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeBackLeft<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.left;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeBackRight<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid) 
        {
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.right;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
#endregion
        
#region Add Node Up, UpLeft, UpRight, UpForward, UpBack
        public static void AddNodeUp<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid) 
        {
            _nodePos += Vector3Int.up;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeUpLeft<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid) 
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.left;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeUpRight<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid) 
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.right;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeUpForward<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.forward;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeUpBack<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid) 
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.back;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
#endregion
        
#region Add Node UpForwardLeft, UpForwardRight, UpBackLeft, UpBackRight
        public static void AddNodeUpForwardLeft<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.left;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeUpForwardRight<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.right;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeUpBackLeft<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.left;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeUpBackRight<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.up;
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.right;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
#endregion
        
#region Add Node Down, DownLeft, DownRight, DownForward, DownBack
        public static void AddNodeDown<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.down;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeDownLeft<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.left;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeDownRight<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.right;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeDownForward<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.forward;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeDownBack<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.back;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
#endregion
        
#region GetNode DownForwardLeft, DownForwardRight, DownBackLeft, DownBackRight
        public static void AddNodeDownForwardLeft<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.left;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeDownForwardRight<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.forward;
            _nodePos += Vector3Int.right;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeDownBackLeft<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.left;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
        
        public static void AddNodeDownBackRight<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            _nodePos += Vector3Int.down;
            _nodePos += Vector3Int.back;
            _nodePos += Vector3Int.right;
            AddNode(_newNode, _nodePos, _gridSize, ref _grid);
        }
#endregion

#region Add/Remove Nodes Methods
        // public static void AddNode<T, U>(U _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid) where T : NodeBase
        // {
        //     int _x = _nodePos.x;
        //     int _y = _nodePos.y;
        //     int _z = _nodePos.z;
        //
        //     if (_x < 0 || _x >= _gridSize.x) return;
        //     if (_y < 0 || _y >= _gridSize.y) return;
        //     if (_z < 0 || _z >= _gridSize.z) return;
        //     
        //     _grid[_z + _gridSize.x * (_x + _gridSize.y * _y)] = _newNode;
        // }
        
        public static void AddNode<T>(T _newNode, Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            int _x = _nodePos.x;
            int _y = _nodePos.y;
            int _z = _nodePos.z;

            if (_x < 0 || _x >= _gridSize.x) return;
            if (_y < 0 || _y >= _gridSize.y) return;
            if (_z < 0 || _z >= _gridSize.z) return;
            
            _grid[_z + _gridSize.x * (_x + _gridSize.y * _y)] = _newNode;
        }
        
        public static void RemoveNode<T>(Vector3Int _nodePos, Vector3Int _gridSize, ref T[] _grid)
        {
            int _x = _nodePos.x;
            int _y = _nodePos.y;
            int _z = _nodePos.z;

            if (_x < 0 || _x >= _gridSize.x) return;
            if (_y < 0 || _y >= _gridSize.y) return;
            if (_z < 0 || _z >= _gridSize.z) return;

            _grid[_z + _gridSize.x * (_x + _gridSize.y * _y)] = default;
        }
#endregion
        
        // public Node NodeFromWorldPoint(Vector3 worldPosition) {
        //     float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        //     float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
        //     percentX = Mathf.Clamp01(percentX);
        //     percentY = Mathf.Clamp01(percentY);
        //
        //     int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
        //     int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
        //     return grid[x,y];
        // }
        
        public static T[] GenerateGridINodes<T>(Vector3Int _gridUnitSize, GameObject _blockPrefab = null, GameObject _parent = null) where T : NodeBase
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
                        Vector3 _position = new Vector3(_x, _y, _z);

                        T _nodeBase;
                        
                        if (_blockPrefab != null)
                        {
                            var _nodeObject = Object.Instantiate(_blockPrefab, _position, Quaternion.identity);

                            if (!_nodeObject.TryGetComponent(out _nodeBase))
                                _nodeBase = _nodeObject.AddComponent<T>();
                            
                            _nodeBase.NodePosition = new Vector3Int(_x, _y, _z);
                        }
                        else
                        {
                            var _emptyNode = new GameObject("Empty Node");
                            
                            _nodeBase = _emptyNode.AddComponent<T>();
                            _nodeBase.NodePosition = new Vector3Int(_x, _y, _z);
                            
                            Undo.RegisterCreatedObjectUndo(_emptyNode, "Node Created");
                        }
                         
                        // if (_parent != null)
                        // {
                        //     _block.transform.position += _parent.transform.position;
                        //     _block.transform.SetParent(_parent.transform);
                        // }
                        //
                        // _block.Init(_block.transform.position, new Vector3Int(_x, _y, _z));

                        _nodes[_z + _gridUnitSize.x * (_x + _gridUnitSize.y * _y)] = _nodeBase;
                    }
                }
            }

            return _nodes;
        }
    }
}
