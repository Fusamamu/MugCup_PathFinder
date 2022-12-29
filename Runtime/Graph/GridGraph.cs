using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEditor.Graphs;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class GridGraph : MonoBehaviour, IGraph<VertexNode>
    {
        public Dictionary<VertexNode, VertexNode[]> Edges { get; private set; }

        [SerializeField] private GridNodeBaseData GridNodeData;

        [SerializeField] private Transform VertexPrefab;

        public GridGraph Initialized(GridNodeBaseData _gridNodeData)
        {
            GridNodeData = _gridNodeData;
            return this;
        }

        public GridGraph ConstructGraph()
        {
            var _firstLevelNodes = GridUtility.GetNodesByLevel(0, GridNodeData.GridSize, GridNodeData.GridNodes);

            foreach (var _node in _firstLevelNodes)
            {
                //if(_node.)
            }
            

            return this;
        }

        public VertexNode[] GetNeighbors(VertexNode _node)
        {
            return null;
        }
        
        public double GetWeightCost(VertexNode _nodeA, VertexNode _nodeB)
        {
            return 0;
        }
    }
}
