using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class VertexNode : MonoBehaviour, INode
    {
#region Node Position Information
        [field: SerializeField] public Vector3Int NodePosition      { get; private set; }
        [field: SerializeField] public Vector3    NodeWorldPosition { get; private set; }
	    
        public INode SetNodePosition(Vector3Int _nodePosition)
        {
            NodePosition = _nodePosition;
            return this;
        }

        public INode SetNodeWorldPosition(Vector3 _worldPosition)
        {
            NodeWorldPosition = _worldPosition;
            return this;
        }
#endregion
       
#region Node Path
        public INode NextNodeOnPath    { get; set; }
        public Vector3Int NextNodePosition  { get; set; }
        public Vector3    ExitPosition      { get; set; }
	    
        public NodeDirection Direction { get; set; }

        public void SetNextNodeOnPath(INode _node)
        {
            NextNodeOnPath   = _node;
            NextNodePosition = _node.NodePosition;
		    
            ExitPosition = (NodeWorldPosition + NextNodeOnPath.NodeWorldPosition) / 2.0f;
        }
	    
        public void SetNodePathDirection(NodeDirection _direction)
        {
            Direction = _direction;
        }
#endregion
   
#region Node Cost
        [field: SerializeField] public int G_Cost { get; set; }
        [field: SerializeField] public int H_Cost { get; set; }
        [field: SerializeField] public int F_Cost => G_Cost + H_Cost;
        [field: SerializeField] public int HeapIndex { get; set; }
	    
        public int CompareTo(GridNode _gridNodeToCompare)
        {
            int _compare = F_Cost.CompareTo(_gridNodeToCompare.F_Cost);
				
            if (_compare == 0) 
                _compare = H_Cost.CompareTo(_gridNodeToCompare.H_Cost);
				
            return -_compare;
        }
#endregion
        
#region Node Neighbors
        public INode NodeParent { get; set; }
        public HashSet<INode> Neighbors { get;  }
#endregion
    }
}
