using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathGizmos : MonoBehaviour
    {
        [SerializeField] private float nodeRadius = 0.05f;
            
        [SerializeField] private Color nodeColor = Color.blue;
        
        private GridNode[] grid;

        public void SetGridRef(GridNode[] _grid)
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
                Gizmos.DrawSphere(_node.NodeGridPosition, nodeRadius);
            }
            
        }
    }
}
