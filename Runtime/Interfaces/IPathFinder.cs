using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IPathFinder<T> where T : INode
    {
        public Vector3Int GridSize  { get; } 
        public T[]        GridNodes { get; } 

        public IEnumerable<T>          FindPath(T _origin, T _target);
        public IEnumerable<Vector3Int> FindPath(Vector3Int _origin, Vector3Int _target);
    }
}
