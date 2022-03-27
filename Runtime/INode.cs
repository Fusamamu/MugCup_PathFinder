using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public class NodeBase : MonoBehaviour, INode
	{
		public INode NodeParent { get; set; }
		
		public Vector3Int NodePosition { get; set; }
		
		public int G_Cost { get; set; }
		public int H_Cost { get; set; }
		
		public int F_Cost => G_Cost + H_Cost;
	}
	
	public interface INode
	{
		public INode NodeParent { get; set; }
		
		public Vector3Int NodePosition { get; set; }
		
		public int G_Cost { get; set; }
		public int H_Cost { get; set; }
		public int F_Cost { get; }

		//public bool Equals(INode _other);

	}
}

