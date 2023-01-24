using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MugCup_PathFinder.Runtime
{
	public struct ProcessPathJob : IJob, IDisposable
	{
		public GridStructure GridStructure;
		
		public NativeList<int3> Result;
		
		public NativeBinaryHeap<NodeCost>    Open;
		public NativeHashMap<int3, NodeCost> Closed;
		
		public int3 StartNodePos;
		public int3 TargetNodePos;

		public NativeList<int3> Neighbors;

		public void Execute()
		{
			if (StartNodePos.x == -1 || TargetNodePos.x == -1) return;

			int3 _boundsMin = new int3(0, 0, 0);
			int3 _boundsMax = new int3
			{
				x = GridStructure.Row,
				y = GridStructure.Level,
				z = GridStructure.Column
			};

			var _currentNode = new NodeCost(StartNodePos, StartNodePos);

			Open.Add(_currentNode);

			while (Open.CurrentItemCount > 0)
			{
				_currentNode = Open.RemoveFirst();

				Closed.Add(_currentNode.Index, _currentNode);

				if (math.all(_currentNode.Index == TargetNodePos))
					break;

				Neighbors.Clear();
				Neighbors.Add(_currentNode.Index.LeftIndex());
				Neighbors.Add(_currentNode.Index.RightIndex());
				Neighbors.Add(_currentNode.Index.Forward());
				Neighbors.Add(_currentNode.Index.Back());

				foreach (var _newIdx in Neighbors)
				{
					if (!math.all(_newIdx >= _boundsMin & _newIdx < _boundsMax)) continue;
					
					if (Closed.ContainsKey(_newIdx)) continue;
					
					//NodeNativeData _neighbor = GridStructure.GetNode(_newIdx);

					var _newCost = new NodeCost(_newIdx, _currentNode.Index);

					// if (!neighbor.walkable || closed.TryGetValue(newIdx, out NodeCost _))
					// {
					// 	continue;
					// }

					int _nodeGCost = _currentNode.GCost + NodeDistance(_currentNode.Index, _newIdx);

					_newCost.GCost = _nodeGCost;
					_newCost.HCost = NodeDistance(_newIdx, TargetNodePos);

					int _oldIdx = Open.IndexOf(_newCost);
					if (_oldIdx >= 0)
					{
						if (_nodeGCost < Open[_oldIdx].GCost)
						{
							Open.RemoveAt(_oldIdx);
							Open.Add(_newCost);
						}
					}
					else
					{
						if (Open.CurrentItemCount < Open.Capacity)
							Open.Add(_newCost);
						else
							return;
					}
					
					
				}
			}
			
			while (!math.all(_currentNode.Index == _currentNode.Origin))
			{
				Result.Add(_currentNode.Index);

				if (!Closed.TryGetValue(_currentNode.Origin, out NodeCost _next))
					return;

				_currentNode = _next;
			}
		}
		
		private int NodeDistance(int3 _nodeA, int3 _nodeB) 
		{
			int3 _d = _nodeA - _nodeB;
			
			int _distX = math.abs(_d.x);
			int _distZ = math.abs(_d.z);

			if (_distX > _distZ)
				return 14 * _distZ + 10 * (_distX - _distZ);
				
			return 14 * _distX + 10 * (_distZ - _distX);
		}

		public void ForceDispose()
		{
			GridStructure.Dispose();
			Result       .Dispose();
			Open         .Dispose();
			Closed       .Dispose();

			Neighbors.Dispose();
		}
		
		public void Dispose()
		{
			GridStructure.Dispose();
			
			if(Result.IsCreated)     Result.Dispose();
			if(Open.Items.IsCreated) Open.Dispose();
			if(Closed.IsCreated)     Closed.Dispose();

			if (Neighbors.IsCreated) Neighbors.Dispose();
		}
	}
}
