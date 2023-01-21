using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IPathFinder<T> where T : INode
    {
        public GridData<T> GridData { get; }

        public void FindPath(PathRequest<Vector3Int> _pathRequest,  Action<PathResult<Vector3Int>> _onPathFound);
    }
}
