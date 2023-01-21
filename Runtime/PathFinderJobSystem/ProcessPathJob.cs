using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public struct ProcessPathJob : IJob
	{
		public GridStructure GridStructure;
		
		public NativeList<int3> Result;
		
		public NativeBinaryHeap<NodeCost>    Open;
		public NativeHashMap<int3, NodeCost> Closed;
		
		public int3 StartNodePos;
		public int3 TargetNodePos;

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

				if (!Closed.TryAdd(_currentNode.Index, _currentNode))
					break;

				if (math.all(_currentNode.Index == TargetNodePos))
				{
					break;
				}

				for (int _xC = -1; _xC <= 1; _xC++)
				{
					for (int _zC = -1; _zC <= 1; _zC++)
					{
						int3 _newIdx = +_currentNode.Index + new int3(_xC, 0, _zC);

						if (math.all(_newIdx >= _boundsMin & _newIdx < _boundsMax))
						{
							NodeNativeData _neighbor = GridStructure.GetNode(_newIdx);

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
								{
									Open.Add(_newCost);
								}
								else
								{
									return;
								}
							}
						}
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
			
		private int NodeDistance (int3 _nodeA, int3 _nodeB) 
		{
			int3 _d = _nodeA - _nodeB;
			
			int _distX = math.abs(_d.x);
			int _distZ = math.abs(_d.z);

			if (_distX > _distZ)
				return 14 * _distZ + 10 * (_distX - _distZ);
				
			return 14 * _distX + 10 * (_distZ - _distX);
		}
	}
}
