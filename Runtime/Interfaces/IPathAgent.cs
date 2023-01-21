using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IPathAgent<T>
    {
        public IPathAgent<T> SetTargetPath(T[] _targetPath);
        
        public IPathAgent<T> SetStartPos (T _startPos);
        public IPathAgent<T> SetTargetPos(T _targetPos);
        
        public IPathAgent<T> RequestPath();
        public IPathAgent<T> StartFollowPath();
        public IPathAgent<T> StopFollowPath();
    }
}
