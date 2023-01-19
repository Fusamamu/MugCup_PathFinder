using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public struct NodeData
    {
        public Vector3Int NodeGridPosition ;
        public Vector3    NodeWorldPosition;
    
        public Vector3Int NextNodePosition;
        public Vector3    ExitPosition    ;
    
        public INode NodeParent;
    
        public NodeDirection Direction;
    }
}
