using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public interface IGraph<T> where T : INode
    {
        public Dictionary<T, T[]> Edges { get; }

        public T[] GetNeighbors(T _node);

        public double GetWeightCost(T _nodeA, T _nodeB);
    }
}
