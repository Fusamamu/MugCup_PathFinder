using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IPathFinderController<T> where T : INode
    {
        public void Initialized();

        public IPathFinderController<T> SelectGridDataNode(GridData<T> _gridData);
        public IPathFinderController<T> InitializePathFinder();
   
        public void RequestPath(PathRequest<T> _request, bool _waitForComplete = false);
        public Task RequestPathAsync(PathRequest<T> _request, Action _onCompleted);
    }
}
