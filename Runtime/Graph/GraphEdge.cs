using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    [Serializable]
    public struct GraphEdge
    {
        [field: SerializeField] public INode From { get; set; }
        [field: SerializeField] public INode To   { get; set; }
        
        [field: SerializeField] public int Weight      { get; set; }
        [field: SerializeField] public bool IsDirected { get; set; }
        
        [field: SerializeField] public string Label    { get; set; }

        public GraphEdge(INode _from, INode _to, int _weight = 0, bool _isDirected = false, string _label = "")
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
