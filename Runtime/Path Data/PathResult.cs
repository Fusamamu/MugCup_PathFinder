using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathResultNodeBase : PathResult<GridNode>
    {
        public PathResultNodeBase(GridNode[] _path, bool _success, Action<GridNode[], bool> _onPathFound) : base(_path, _success, _onPathFound)
        {
        }
    }

    public class PathResult<T> where T : INode
    {
        public readonly T[] Path;
        
        public readonly bool Success;
        
        public readonly Action<T[], bool> OnPathFound;

        public PathResult (T[] _path, bool _success, Action<T[], bool> _onPathFound)
        {
            Path        = _path       ;
            Success     = _success    ;
            OnPathFound = _onPathFound;
        }
    }

    // public struct NodeData
    // {
    //     public Vector3Int NodeGridPosition ;
    //     public Vector3    NodeWorldPosition;
		  //
    //     public Vector3Int NextNodePosition;
    //     public Vector3    ExitPosition    ;
    //
    //     public INode NodeParent;
    //
    //     public NodeDirection Direction;
    // }
}
