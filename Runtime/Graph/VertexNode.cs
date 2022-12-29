using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class VertexNode : INode
    {
        public INode NodeParent { get; set; }
        
        public Vector3Int NodePosition      { get; }
        public Vector3    NodeWorldPosition { get; }
        
        public Vector3Int NextNodePosition { get; set; }
        public Vector3 ExitPosition { get; set; }
        
        public int G_Cost { get; set; }
        public int H_Cost { get; set; }
        public int F_Cost { get; }
        
        public INode NorthNode { get; }
        public INode SouthNode { get; }
        public INode WestNode  { get; }
        public INode EastNode  { get; }
        
        public INode NextNodeOnPath { get; set; }
        
        public NodeDirection Direction { get; set; }
        
        public void SetNextNodeOnPath(INode _node)
        {
        }
        public void SetNodePathDirection(NodeDirection _direction)
        {
        }
        
        public INode GrowPathTo(INode _neighbor, NodeDirection _direction)
        {
            return null;
        }
    }
}
