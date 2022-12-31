using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public class GridNode : MonoBehaviour, INode, IHeapItem<GridNode>, IObstacle
	{
		[field: SerializeField] public bool IsObstacle { get; protected set; } = true;
		
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

#region Occupy Area
		[field: SerializeField] public Vector3Int OccupiedDimension { get; private set; }

		public void AddSelfToGrid(GridNodeData _gridNodeData)
		{
			var _width = Mathf.Abs(OccupiedDimension.x);
			var _depth = Mathf.Abs(OccupiedDimension.z);
			
			for (var _i = 0; _i < _width + 1; _i++)
			{
				for (var _j = 0; _j < _depth + 1; _j++)
				{
					int _x = _i;
					int _z = _j;

					if (OccupiedDimension.x < 0)
						_x = _i * -1;

					if (OccupiedDimension.z < 0)
						_z = _j * -1;
					
					var _targetPos = NodePosition + new Vector3Int(_x, 0, _z);
					GridUtility.AddNode(this, _targetPos, _gridNodeData.GridSize, ref _gridNodeData.GridNodes);

					if (this.gameObject.name == "TestStair")
					{
						Debug.Log(_targetPos);
					}
				}
			}
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
	    public HashSet<INode> Neighbors { get; }

	    public void SetNeighbors<T>(GridData<T> _gridNodeData) where T : INode
	    {
		    var _neighbors = GridUtility.GetAdjacentNodes8Dir(NodePosition, _gridNodeData.GridSize, _gridNodeData.GridNodes);

		    foreach (var _node in _neighbors)
			    Neighbors.Add(_node);
	    }
	    
	    public INode NorthNode { get; private set; }
	    public INode SouthNode { get; private set; }
	    public INode WestNode  { get; private set; }
	    public INode EastNode  { get; private set; }

	    public void SetEachNeighbor<T>(GridData<T> _gridNodeData) where T : INode
	    {
		    NorthNode = GridUtility.GetNodeForward(NodePosition, _gridNodeData.GridSize, _gridNodeData.GridNodes);
		    SouthNode = GridUtility.GetNodeBack   (NodePosition, _gridNodeData.GridSize, _gridNodeData.GridNodes);
		    WestNode  = GridUtility.GetNodeLeft   (NodePosition, _gridNodeData.GridSize, _gridNodeData.GridNodes);
		    EastNode  = GridUtility.GetNodeRight  (NodePosition, _gridNodeData.GridSize, _gridNodeData.GridNodes);
	    }
#endregion
	}
}
