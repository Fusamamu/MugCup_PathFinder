using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public interface INode
	{
		public Vector3Int NodeGridPosition      { get; }
		public Vector3    NodeWorldPosition { get; }
		
		public void SetNextNodeOnPath   (INode _node);
		public void SetNodePathDirection(NodeDirection _direction);
		
		public Vector3Int NextNodePosition  { get; set; }
		public Vector3    ExitPosition      { get; set; }

		public int G_Cost { get; set; }
		public int H_Cost { get; set; }
		public int F_Cost { get; }
		
		/*Neighbor references*/
		public INode NodeParent { get; set; }
		public HashSet<INode> Neighbors { get; }

		public IEnumerable<T> GetNeighbors<T>() where T : INode;
		public void SetNeighbors<T>(IEnumerable<T> _neighbors) where T : INode;

		public INode NextNodeOnPath { get; set; }
		public NodeDirection Direction { get; set; }
	}
}

