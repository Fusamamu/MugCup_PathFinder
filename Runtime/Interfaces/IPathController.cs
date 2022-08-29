using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IPathFinderController<T> where T : NodeBase
    {
        public void Initialized();

        public IPathFinderController<T> SelectGridDataNode(GridNodeData<T> _gridNodeData);
        public IPathFinderController<T> InitializePathFinder();
   
        public void RequestPath(PathRequest<T> _request, bool _waitForComplete = false);
    }
}
