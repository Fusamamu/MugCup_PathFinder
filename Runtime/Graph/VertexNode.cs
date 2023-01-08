using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class VertexNode : MonoBehaviour, INode
    {
        [field: SerializeField] public int Id       { get; set; }
        [field: SerializeField] public string Label { get; set; }
        [field: SerializeField] public bool Visited { get; set; }

        [field: SerializeField] public List<GraphEdge> Edges { get; private set; } = new List<GraphEdge>();

        public void AddEdges(IEnumerable<GraphEdge> _edges)
        {
            foreach (var _edge in _edges)
            {
                var _node = _edge.To;
                
                Neighbors.Add(_node);
                Edges    .Add(_edge);
            }
        }
        
#region Node Position Information
        [field: SerializeField] public Vector3Int NodeGridPosition      { get; private set; }
        [field: SerializeField] public Vector3    NodeWorldPosition { get; private set; }
	    
        public INode SetNodePosition(Vector3Int _nodePosition)
        {
            NodeGridPosition = _nodePosition;
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
            NextNodePosition = _node.NodeGridPosition;
		    
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
        public HashSet<INode> Neighbors { get; private set; } = new HashSet<INode>();
        
        public IEnumerable<T> GetNeighbors<T>() where T : INode
        {
            foreach (var _edge in Edges)
            {
                yield return (T)_edge.To;
            }
        }
        
        public void SetNeighbors<T>(IEnumerable<T> _neighbors) where T : INode
        {
            throw new NotImplementedException();
        }
#endregion

        private void OnDrawGizmosSelected()
        {
            foreach (var _edge in Edges)
            {
                var _startPos  = _edge.From.NodeWorldPosition;
                var _targetPos = _edge.To.NodeWorldPosition;
                
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(_startPos, _targetPos);
            }
        }
    }
}
