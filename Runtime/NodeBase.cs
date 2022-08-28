using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public class NodeBase : MonoBehaviour, INode, IHeapItem<NodeBase>
	{
	    public INode NodeParent { get; set; }
			
	    public Vector3Int NodePosition { get; set; }
	    public Vector3    NodeWorldPosition => nodeWorldPosition;
	    
	    public Vector3Int NextNodePosition  { get; set; }

	    public int G_Cost { get; set; }
	    public int H_Cost { get; set; }
	    public int F_Cost => G_Cost + H_Cost;
			
	    public int HeapIndex { get; set; }

	    [SerializeField] private Vector3 nodeWorldPosition;
	    
	    public INode NorthNode { get; private set; }
	    public INode SouthNode { get; private set; }
	    public INode WestNode  { get; private set; }
	    public INode EastNode  { get; private set; }

	    public INode NextNodeOnPath    { get; set; }
	    public NodeDirection Direction { get; set; }

	    public NodeDirection DIR;
			
	    public int CompareTo(NodeBase _nodeToCompare)
	    {
	        int _compare = F_Cost.CompareTo(_nodeToCompare.F_Cost);
				
	        if (_compare == 0) 
	        {
	            _compare = H_Cost.CompareTo(_nodeToCompare.H_Cost);
	        }
				
	        return -_compare;
	    }

	    public void SetNodeWorldPosition(Vector3 _worldPosition)
	    {
		    nodeWorldPosition = _worldPosition;
	    }

	    public void SetNorthNode(INode _node) => NorthNode = _node;
	    public void SetSouthNode(INode _node) => SouthNode = _node;
	    public void SetWestNode (INode _node) => WestNode  = _node;
	    public void SetEastNode (INode _node) => EastNode  = _node;

	    public void SetNextNodeOnPath(INode _node)
	    {
		    NextNodeOnPath = _node;
	    }
	    public void SetNodePathDirection(NodeDirection _direction)
	    {
		    Direction = _direction;
		    DIR = _direction;
		    
		    Debug.Log(DIR);
	    }

	    public INode GrowPathTo(INode _neighbor, NodeDirection _direction)
	    {
		    _neighbor.NextNodeOnPath = this;
		    _neighbor.Direction      = _direction;

		    return _neighbor;
	    }
	    
	    // GameTile GrowPathTo (GameTile neighbor, Direction direction) {
		   //  if (neighbor == null || neighbor.HasPath) {
			  //   return null;
		   //  }
		   //  neighbor.distance = distance + 1;
		   //  neighbor.nextOnPath = this;
		   //  neighbor.ExitPoint =
			  //   neighbor.transform.localPosition + direction.GetHalfVector();
		   //  neighbor.PathDirection = direction;
		   //  return
			  //   neighbor.Content.Type != GameTileContentType.Wall ? neighbor : null;
	    // }
	}
}
