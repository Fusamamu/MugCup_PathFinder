using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	// public class NodeBase : MonoBehaviour, INode, IHeapItem<NodeBase>
	// {
	// 	public INode NodeParent { get; set; }
	// 	
	// 	public Vector3Int NodePosition { get; set; }
	// 	
	// 	public int G_Cost { get; set; }
	// 	public int H_Cost { get; set; }
	// 	
	// 	public int F_Cost => G_Cost + H_Cost;
	// 	
	// 	public int HeapIndex { get; set; }
	// 	
	// 	public int CompareTo(NodeBase _nodeToCompare)
	// 	{
	// 		int _compare = F_Cost.CompareTo(_nodeToCompare.F_Cost);
	// 		
	// 		if (_compare == 0) 
	// 		{
	// 			_compare = H_Cost.CompareTo(_nodeToCompare.H_Cost);
	// 		}
	// 		
	// 		return -_compare;
	// 	}
	// }
	
	public interface INode
	{
		public INode NodeParent { get; set; }
		
		public Vector3Int NodePosition      { get; set; }
		public Vector3    NodeWorldPosition { get; }
		
		public Vector3Int NextNodePosition  { get; set; }

		public int G_Cost { get; set; }
		public int H_Cost { get; set; }
		public int F_Cost { get; }

		public INode NorthNode { get; }
		public INode SouthNode { get; }
		public INode WestNode  { get; }
		public INode EastNode  { get; }
		
		public INode NextNodeOnPath { get; set; }

		public NodeDirection Direction { get; set; }

		public void SetNextNodeOnPath(INode _node);
		public void SetNodePathDirection(NodeDirection _direction);

		public INode GrowPathTo(INode _neighbor, NodeDirection _direction);

		//public bool Equals(INode _other);

	}
}

