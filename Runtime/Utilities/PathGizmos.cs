using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime.Utilities
{
    public class PathGizmos : MonoBehaviour
    {
        [SerializeField] private float nodeRadius = 0.05f;
            
        [SerializeField] private Color nodeColor = Color.blue;
        
        private NodeBase[] grid;

        public void SetGridRef(NodeBase[] _grid)
        {
            grid = _grid;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(Vector3.zero, nodeRadius);
            
            if(grid == null || grid.Length == 0) return;
            
            
            foreach (var _node in grid)
            {
                Gizmos.DrawSphere(_node.NodePosition, nodeRadius);
            }
            
        }
    }
}
