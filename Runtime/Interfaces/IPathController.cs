using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IPathFinderController<T> where T : NodeBase
    {
        public void Initialized(GridNodeData<T> _gridNodeData);
        public void RequestPath(PathRequest<T> _request);
    }
}
