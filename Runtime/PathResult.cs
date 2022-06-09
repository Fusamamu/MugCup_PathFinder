using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathResultVec3 : PathResult<Vector3>
    {
        public PathResultVec3(Vector3[] _path, bool _success, Action<Vector3[], bool> _onPathFound) : base(_path, _success, _onPathFound)
        {
        }
    }

    public class PathResultNodeBase : PathResult<NodeBase>
    {
        public PathResultNodeBase(NodeBase[] _path, bool _success, Action<NodeBase[], bool> _onPathFound) : base(_path, _success, _onPathFound)
        {
        }
    }

    public class PathResult<T> 
    {
        public T[] Path;
        
        public bool Success;
        
        public Action<T[], bool> OnPathFound;

        public PathResult (T[] _path, bool _success, Action<T[], bool> _onPathFound)
        {
            Path        = _path       ;
            Success     = _success    ;
            OnPathFound = _onPathFound;
        }
    }
}
