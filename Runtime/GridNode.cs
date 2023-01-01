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

		public IEnumerable<(Vector3, Vector3Int)> GenerateVertexNodePositions()
		{
			//Temp don't know why onvalidate not called
			if (OccupiedPositions == null || OccupiedPositions.Count == 0)
			{
				OccupiedPositions = new List<Vector3>
				{
					NodeWorldPosition
				};
			}

			int _i = 0;
			foreach (var _pos in OccupiedPositions)
			{
				var _targetVertexWorldPos = _pos + Vector3.up / 2;

				if (LocalVertexConnectors.Count != 0)
				{
					_targetVertexWorldPos = _pos + LocalVertexConnectors[_i];
				}
				_i++;
				
				var _targetVertexGridPos  =  new Vector3Int((int)_pos.x, (int)_pos.y, (int)_pos.z) + Vector3Int.up;

				yield return (_targetVertexWorldPos, _targetVertexGridPos);
			}
		}
		
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
				if (OccupiedPositions == null || OccupiedPositions.Count == 0)
				{
					OccupiedPositions = new List<Vector3>
					{
						Vector3.zero
					};
				}
				
				if (LocalVertexConnectors == null || LocalVertexConnectors.Count == 0)
				{
					LocalVertexConnectors = new List<Vector3>
					{
						Vector3.zero
					};
				}
				
				
				for (var _i = 0; _i < OccupiedPositions.Count; _i++)
					yield return OccupiedPositions[_i] + LocalVertexConnectors[_i];
			}
		}

#region Occupy Area
		[field: SerializeField] public Vector3 OccupiedDimension { get; private set; }
		[field: SerializeField] public List<Vector3> OccupiedPositions = new List<Vector3>();

		public int Width => (int)Mathf.Abs(OccupiedDimension.x) + 1;
		public int Depth => (int)Mathf.Abs(OccupiedDimension.z) + 1;
		
		private void OnValidate()
		{
			UpdateOccupiedPositions();
		}

		private void UpdateOccupiedPositions()
		{
			OccupiedPositions.Clear();
			
			for (var _i = 0; _i < Width; _i++)
				for (var _j = 0; _j < Depth; _j++)
				{
					int _x = OccupiedDimension.x < 0 ? _i * -1 : _i;
					int _z = OccupiedDimension.z < 0 ? _j * -1 : _j;
					
					var _targetPos = NodePosition + new Vector3(_x, 0, _z);
					OccupiedPositions.Add(_targetPos);
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
#endregion

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
