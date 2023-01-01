using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	[Serializable]
	public struct NodeData
	{
		public Vector3    WorldPos;
		public Vector3Int GridPos;
		public Vector3Int GridPosLocal;
	}

	public class GridNode : MonoBehaviour, INode, IHeapItem<GridNode>, IObstacle
	{
		[field: SerializeField] public bool IsObstacle       { get; protected set; } = true;
		[field: SerializeField] public bool IsVertexNodeInit { get; protected set; } = false;

		public void SetVertexNodeInit(bool _value)
		{
			IsVertexNodeInit = _value;
		}

		public IEnumerable<(Vector3, Vector3Int)> GenerateVertexNodePositions()
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

					var _targetVertexWorldPos = NodeWorldPosition + Vector3.up / 2 + new Vector3(_x, 0, _z);
					var _targetVertexGridPos  = NodePosition      + Vector3Int.up  + new Vector3Int(_x, 0, _z);

					yield return (_targetVertexWorldPos, _targetVertexGridPos);
				}
			}
		}

		public IEnumerable<Vector3> NodeConnectorsWorldPos 
		{
			get
			{
				foreach (var _connect in NodeConnects)
					yield return NodeWorldPosition + _connect;
			}
		}

		public List<Vector3> NodeConnects 
		{
			get
			{
				if (NodeConnectors == null || NodeConnectors.Count == 0)
				{
					NodeConnectors = new List<Vector3>
					{
						Vector3.forward,
						Vector3.right,
						Vector3.back,
						Vector3.left
					};
				}
				
				return NodeConnectors;
			}
			
			protected set => NodeConnectors = value;
		}

		[SerializeField] private List<Vector3> NodeConnectors = new List<Vector3>();
		
		

		public IEnumerable<Vector3> EdgeConnects 
		{
			get
			{
				foreach (var _pos in EdgeConnectors)
				{
					yield return _pos + NodeWorldPosition;
				}
			}
		}
			
		[SerializeField] private List<Vector3> EdgeConnectors = new List<Vector3>();
 

#region Occupy Area
		[field: SerializeField] public Vector3 OccupiedDimension { get; private set; }

		public List<Vector3> AllOccupiedPos {
			get
			{
				return OccupiedPositions;
			}
		}

		[SerializeField] private List<Vector3> OccupiedPositions = new List<Vector3>();
		
		

		private void OnValidate()
		{
			UpdateOccupiedPositions();
		}

		private void UpdateOccupiedPositions()
		{
			OccupiedPositions.Clear();
			
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
					
					var _targetPos = NodePosition + new Vector3(_x, 0, _z);

					OccupiedPositions.Add(_targetPos);
				}
			}
		}

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
				}
			}
		}
#endregion

		public void RotateClockwise()
		{
			transform.Rotate(new Vector3(0, 90, 0));
			
			Matrix4x4 _rotation = Matrix4x4.Rotate(Quaternion.Euler(0, 90, 0));
			OccupiedDimension = _rotation.MultiplyPoint(OccupiedDimension);
			OccupiedDimension = new Vector3(Mathf.Round(OccupiedDimension.x), 0, Mathf.Round(OccupiedDimension.z));

			for(int _i = 0; _i < NodeConnectors.Count; _i++)
			{
				var _newVec = _rotation.MultiplyPoint(NodeConnectors[_i]);
				NodeConnectors[_i] = new Vector3(Mathf.Round(_newVec.x), NodeConnectors[_i].y, Mathf.Round(_newVec.z));
			}
			
			UpdateOccupiedPositions();
		}

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
		
		private void OnDrawGizmosSelected()
		{
			if (NodeConnectors != null && NodeConnectors.Count > 0)
			{
				foreach (var _node in NodeConnectors)
				{
					Gizmos.color = Color.green;
					var _size = 1.1f;
					Gizmos.DrawWireCube(_node + NodeWorldPosition, new Vector3(_size, _size, _size));
				}
			}
			
			foreach (var _pos in OccupiedPositions)
			{
				var _size = 1.0f;
				Gizmos.color = Color.red;
				Gizmos.DrawWireCube(_pos, new Vector3(_size, _size, _size));
			}

			foreach (var _edgeConnect in EdgeConnects)　　
			{
				var _size = 0.2f;
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(_edgeConnect, new Vector3(_size, _size, _size));
			}
		}
	}
}
