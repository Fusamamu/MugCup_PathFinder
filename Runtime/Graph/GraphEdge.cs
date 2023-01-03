using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    [Serializable]
    public struct GraphEdge
    {
        [field: SerializeField] public VertexNode From { get; set; }
        [field: SerializeField] public VertexNode To   { get; set; }
        
        [field: SerializeField] public int Weight      { get; set; }
        [field: SerializeField] public bool IsDirected { get; set; }
        
        [field: SerializeField] public string Label    { get; set; }

        public GraphEdge(VertexNode _from, VertexNode _to, int _weight = 0, bool _isDirected = false, string _label = "")
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
