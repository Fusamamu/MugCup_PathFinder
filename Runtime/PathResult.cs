using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public struct PathResult 
    {
        public Vector3[] Path;
        
        public bool Success;
        
        public readonly Action<Vector3[], bool> OnPathFound;

        public PathResult (Vector3[] _path, bool _success, Action<Vector3[], bool> _onPathFound)
        {
            Path     = _path    ;
            Success  = _success ;
            
            OnPathFound = _onPathFound;
        }
    }
}
