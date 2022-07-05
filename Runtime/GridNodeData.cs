using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    [Serializable]
    public class GridNodeData
    {
        public NodeBase[] GridNodes;
        public Vector3Int GridSize ;
    }
}
