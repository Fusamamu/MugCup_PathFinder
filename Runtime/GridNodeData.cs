using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    // [Serializable]
    // public class GridNodeBaseData : GridNodeData<NodeBase>
    // {
    //     
    // }

    [Serializable]
    public class GridNodeData<T> where T : NodeBase
    {
        public T[]        GridNodes;
        public Vector3Int GridSize ;
    }
}
