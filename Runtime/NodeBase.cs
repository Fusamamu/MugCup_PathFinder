using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public class NodeBase : MonoBehaviour, INode, IHeapItem<NodeBase>
	{
	    public INode NodeParent { get; set; }
			
	    public Vector3Int NodePosition { get; set; }
			
	    public int G_Cost { get; set; }
	    public int H_Cost { get; set; }
			
	    public int F_Cost => G_Cost + H_Cost;
			
	    public int HeapIndex { get; set; }
			
	    public int CompareTo(NodeBase _nodeToCompare)
	    {
	        int _compare = F_Cost.CompareTo(_nodeToCompare.F_Cost);
				
	        if (_compare == 0) 
	        {
	            _compare = H_Cost.CompareTo(_nodeToCompare.H_Cost);
	        }
				
	        return -_compare;
	    }
	}
}
