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

        [SerializeField] private GridVertexData   GridVertexData;
        [SerializeField] private GridNodeBaseData GridNodeData;

        [SerializeField] private VertexNode VertexPrefab;

        public GridGraph Initialized(GridNodeBaseData _gridNodeData)
        {
            GridNodeData = _gridNodeData;
            return this;
        }

        public void GenerateValidVertices()
        {
            foreach (var _node in GridNodeData.GridNodes)
            {
                if(_node == null) continue;

                if (!GridUtility.HasNodeOnTop(_node, GridNodeData.GridSize, GridNodeData.GridNodes))
                {
                    //Gen Vertex
                    var _targetVertexPos = _node.NodeWorldPosition + Vector3.up;
                    var _vertexNode = Instantiate(VertexPrefab, _targetVertexPos, Quaternion.identity);
                }
            }
        }

        public GridGraph ConstructGraph()
        {
            var _firstLevelNodes = GridUtility.GetNodesByLevel(1, GridNodeData.GridSize, GridNodeData.GridNodes);

            foreach (var _node in _firstLevelNodes)
            {
                if(_node != null) continue;

                var _vertexNode = Instantiate(VertexPrefab, _node.NodeWorldPosition, Quaternion.identity);
                
                Edges.Add(_vertexNode, null);

             
            }
            

            return this;
        }

        // private VertexNode CreateVertex()
        // {
        //     
        // }

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
