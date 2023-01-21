using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public struct NodeCost : IEquatable<NodeCost>, IComparable<NodeCost>
	{
		public int FCost => GCost + HCost;
		public int GCost;
		public int HCost;
			
		public int3 Index;
		public int3 Origin;

		public NodeCost(int3 _i, int3 _origin)
		{
			Index = _i;
				
			Origin = _origin;
				
			GCost = 0;
			HCost = 0;
		}

		public int CompareTo(NodeCost _other)
		{
			int _compare = FCost.CompareTo(_other.FCost);
				
			if (_compare == 0)
				_compare = HCost.CompareTo(_other.HCost);
				
			return -_compare;
		}

		public bool Equals(NodeCost _other)
		{
			var _b =  Index == _other.Index;
			return math.all(_b);
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode();
		}
	}
}
