using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IPathFinderController<T> where T : INode
    {
        public IPathFinderController<T> SetGridData(GridData<T> _gridData);
        public IPathFinderController<T> SetPathFinder();
        
        public void Initialized();
        public void RequestPath(PathRequest<Vector3Int> _request, bool _waitForComplete = false);
    }
}
