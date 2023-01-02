using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public class GridNode : MonoBehaviour, INode, IHeapItem<GridNode>, IObstacle
	{
		[field: SerializeField] public bool IsObstacle       { get; protected set; } = true;
		[field: SerializeField] public bool IsVertexNodeInit { get; protected set; } = false;
		
		public void SetVertexNodeInit(bool _value)
		{
			IsVertexNodeInit = _value;
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
		[field: SerializeField] public INode NodeParent { get; set; }
		[field: SerializeField] public HashSet<INode> Neighbors { get; }

	    public void SetNeighbors<T>(GridData<T> _gridNodeData) where T : INode
	    {
		    var _neighbors = GridUtility.GetAdjacentNodes8Dir(NodeGridPosition, _gridNodeData.GridSize, _gridNodeData.GridNodes);

		    foreach (var _node in _neighbors)
			    Neighbors.Add(_node);
	    }
	    
	    public INode NorthNode { get; private set; }
	    public INode SouthNode { get; private set; }
	    public INode WestNode  { get; private set; }
	    public INode EastNode  { get; private set; }

	    public void SetEachNeighbor<T>(GridData<T> _gridNodeData) where T : INode
	    {
		    NorthNode = GridUtility.GetNodeForward(NodeGridPosition, _gridNodeData.GridSize, _gridNodeData.GridNodes);
		    SouthNode = GridUtility.GetNodeBack   (NodeGridPosition, _gridNodeData.GridSize, _gridNodeData.GridNodes);
		    WestNode  = GridUtility.GetNodeLeft   (NodeGridPosition, _gridNodeData.GridSize, _gridNodeData.GridNodes);
		    EastNode  = GridUtility.GetNodeRight  (NodeGridPosition, _gridNodeData.GridSize, _gridNodeData.GridNodes);
	    }
#endregion
		
#region Occupy Area
		[field: SerializeField] public Vector3 OccupiedDimension { get; private set; }
		[field: SerializeField] public List<Vector3> OccupiedPositions = new List<Vector3>();

		protected Dictionary<Vector3, Vector3> OccupiedToVertexTable = new Dictionary<Vector3, Vector3>();

		public int Width => (int)Mathf.Abs(OccupiedDimension.x) + 1;
		public int Depth => (int)Mathf.Abs(OccupiedDimension.z) + 1;
		
		private void OnValidate()
		{
			UpdateOccupiedPositions();
			UpdateLocalVertexPositions();
		}

		private void UpdateOccupiedPositions()
		{
			OccupiedPositions.Clear();
			
			for (var _i = 0; _i < Width; _i++)
				for (var _j = 0; _j < Depth; _j++)
				{
					int _x = OccupiedDimension.x < 0 ? _i * -1 : _i;
					int _z = OccupiedDimension.z < 0 ? _j * -1 : _j;
					
					var _targetPos = NodeGridPosition + new Vector3(_x, 0, _z);
					OccupiedPositions.Add(_targetPos);
				}
		}

		public void UpdateLocalVertexPositions()
		{
			for (var _i = 0; _i < OccupiedPositions.Count; _i++)
			{
				var _occupiedPos = OccupiedPositions[_i];


				var _vertexPos = _i < LocalVertexConnectors.Count ? LocalVertexConnectors[_i] : new Vector3(0, 0.5f, 0);

				if (!OccupiedToVertexTable.ContainsKey(_occupiedPos))
				{
					OccupiedToVertexTable.Add(_occupiedPos, _vertexPos);
				}
				else
				{
					OccupiedToVertexTable[_occupiedPos] = _vertexPos;
				}
			}
		}

		public void AddSelfToGrid(GridNodeData _gridNodeData)
		{
			foreach (var _position in OccupiedPositions)
			{
				var _targetPos = new Vector3Int((int)_position.x, (int)_position.y, (int)_position.z);
				GridUtility.AddNode(this, _targetPos, _gridNodeData.GridSize, ref _gridNodeData.GridNodes);
			}
		}

		public void RotateClockwise()
		{
			transform.Rotate(new Vector3(0, 90, 0));
			
			Matrix4x4 _rotation = Matrix4x4.Rotate(Quaternion.Euler(0, 90, 0));
			OccupiedDimension = _rotation.MultiplyPoint(OccupiedDimension);
			OccupiedDimension = new Vector3(Mathf.Round(OccupiedDimension.x), 0, Mathf.Round(OccupiedDimension.z));

			for(int _i = 0; _i < LocalNodeConnectors.Count; _i++)
			{
				var _newVec = _rotation.MultiplyPoint(LocalNodeConnectors[_i]);
				LocalNodeConnectors[_i] = new Vector3(Mathf.Round(_newVec.x), LocalNodeConnectors[_i].y, Mathf.Round(_newVec.z));
			}
			
			UpdateOccupiedPositions();
		}
#endregion
		
		
		[field: SerializeField] private List<Vector3> LocalNodeConnectors = new List<Vector3>();
		
		public IEnumerable<Vector3> WorldNodeConnector 
		{
			get
			{
				foreach (var _pos in LocalNodeConnectors)
					yield return _pos + NodeWorldPosition;
			}
		}

		public int ConnectorCount => LocalNodeConnectors.Count;

		public void Connect4Directions()
		{
			LocalNodeConnectors = new List<Vector3>
			{
				Vector3.forward,
				Vector3.right,
				Vector3.back,
				Vector3.left
			};
		}

		[SerializeField] private List<Vector3> LocalEdgeConnectors = new List<Vector3>();
		
		public IEnumerable<Vector3> WorldEdgeConnectors 
		{
			get
			{
				foreach (var _pos in LocalEdgeConnectors)
					yield return _pos + NodeWorldPosition;
			}
		}

		[SerializeField] private List<Vector3> LocalVertexConnectors = new List<Vector3>();
		
		public IEnumerable<Vector3> WorldVertexConnectors 
		{
			get
			{
				foreach (var _kvp in OccupiedToVertexTable)
				{
					var _occupiedPos = _kvp.Key;
					var _vertexPos   = _kvp.Value;

					var _targetVertexWorldPos = _occupiedPos + _vertexPos;

					yield return _targetVertexWorldPos;
				}
			}
		}
		
		public IEnumerable<(Vector3, Vector3Int)> GenerateVertexNodePositions()
		{
			foreach (var _kvp in OccupiedToVertexTable)
			{
				var _occupiedPos = _kvp.Key;
				var _vertexPos   = _kvp.Value;

				var _targetVertexWorldPos = _occupiedPos + _vertexPos;
				var _targetVertexGridPos  =  new Vector3Int((int)_occupiedPos.x, (int)_occupiedPos.y, (int)_occupiedPos.z) + Vector3Int.up;
				
				yield return (_targetVertexWorldPos, _targetVertexGridPos);
			}
		}

		public void SetDefaultLocalVertexPositions()
		{
			LocalVertexConnectors = new List<Vector3>();

			foreach (var _pos in OccupiedPositions)
				LocalVertexConnectors.Add(new Vector3(0, 0.5f, 0));
		}
		
		private void OnDrawGizmosSelected()
		{
			if (LocalNodeConnectors != null && LocalNodeConnectors.Count > 0)
			{
				foreach (var _node in LocalNodeConnectors)
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

			// foreach (var _edgeConnect in WorldEdgeConnectors)　　
			// {
			// 	var _size = 0.2f;
			// 	Gizmos.color = Color.blue;
			// 	Gizmos.DrawCube(_edgeConnect, new Vector3(_size, _size, _size));
			// }
			
			foreach (var _vertexConnect in WorldVertexConnectors)　　
			{
				var _size = 0.2f;
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(_vertexConnect, new Vector3(_size, _size, _size));
			}
		}
	}
}
