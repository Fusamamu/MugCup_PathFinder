using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    [Serializable]
    public class GraphEdgeVertexNode : GraphEdge<VertexNode>
    {
        public GraphEdgeVertexNode(VertexNode _from, VertexNode _to, int _weight = 0, bool _isDirected = false, string _label = "") 
            : base(_from, _to, _weight, _isDirected, _label)
        {
        }
    }

    [Serializable]
    public class GraphEdge<T> where T : INode
    {
        [field: SerializeField] public T From { get; set; }
        [field: SerializeField] public T To   { get; set; }
        
        [field: SerializeField] public int Weight      { get; set; }
        [field: SerializeField] public bool IsDirected { get; set; }
        
        [field: SerializeField] public string Label    { get; set; }

        public GraphEdge(T _from, T _to, int _weight = 0, bool _isDirected = false, string _label = "")
        {
            From       = _from;
            To         = _to;
            Weight     = _weight;
            IsDirected = _isDirected;
            Label      = _label;
        }

        public override string ToString()
        {
            return $"{From} --{Weight}--> {To}";
        }
    }
}
