using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MugCup_PathFinder.Runtime
{
    public class GridGraph : MonoBehaviour, IGraph<VertexNode>
    {
        public Dictionary<VertexNode, VertexNode[]> Edges { get; private set; }

        [SerializeField] private GridNodeData GridData;
        [SerializeField] private GridVertexData   GridVertexData;

        [SerializeField] private VertexNode VertexPrefab;
        [SerializeField] private Transform VertexParent;

        [Header("Debug")]
        [SerializeField] private bool IsDebug;
        [SerializeField] private bool DisplayBezier;
        [SerializeField] private bool DisplayLine;
        [SerializeField] private float EdgeWidth;
        
        public Vector3 StartTangent;
        public Vector3 EndTangent;

        private void OnValidate()
        {
            if (VertexParent == null)
            {
                var _vertexParentObj = new GameObject("#Vertex Parent")
                {
                    transform = { position = Vector3.zero }
                };

                VertexParent = _vertexParentObj.transform;
            }

            if (Edges == null)
            {
                MapGraph();
            }
        }

        public GridGraph Initialized(GridNodeData _gridData)
        {
            GridData = _gridData;
            return this;
        }

        public void GenerateValidVertices()
        {
            GridVertexData
                .InitializeGridUnitSize(GridData.GridSize)
                .InitializeGridArray();
            
            foreach (var _node in GridData.GridNodes)
            {
                if(_node == null) continue;

                if (!GridUtility.HasNodeOnTop(_node, GridData.GridSize, GridData.GridNodes))
                {
                    var _targetVertexPos = _node.NodeWorldPosition + Vector3.up / 2;
                    
                    var _vertexNode = Instantiate(VertexPrefab, _targetVertexPos, Quaternion.identity, VertexParent);
                    
                    _vertexNode.SetNodePosition     (_node.NodePosition + Vector3Int.up);
                    _vertexNode.SetNodeWorldPosition(_targetVertexPos);
                    
                    GridVertexData.AddNode(_vertexNode, _vertexNode.NodePosition);
                }
                else
                {
                    var _topNode = GridUtility.GetNodeUp(_node.NodePosition, GridData.GridSize, GridData.GridNodes);

                    if (!_topNode.IsObstacle)
                    {
                        var _targetVertexPos = _node.NodeWorldPosition + Vector3.up / 2;
                    
                        var _vertexNode = Instantiate(VertexPrefab, _targetVertexPos, Quaternion.identity, VertexParent);
                    
                        _vertexNode.SetNodePosition     (_node.NodePosition + Vector3Int.up);
                        _vertexNode.SetNodeWorldPosition(_targetVertexPos);
                    
                        GridVertexData.AddNode(_vertexNode, _vertexNode.NodePosition);
                    }
                }
            }
           
            MapGraph();

            #if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorUtility.SetDirty(GridVertexData);
            #endif
        }

        private void MapGraph()
        {
            Edges = new Dictionary<VertexNode, VertexNode[]>();
            
            foreach (var _vertex in GridVertexData.GridNodes)
            {
                if(_vertex == null) continue;
                
                if (!Edges.ContainsKey(_vertex))
                {
                    var _vertices = GridUtility
                        .GetAdjacentNodes8Dir(_vertex, GridVertexData.GridSize, GridVertexData.GridNodes)
                        .Where(_v => _v != null)
                        .ToArray();
                    
                    Edges.Add(_vertex, _vertices);
                }
            }
        }

        public void ClearVertexData()
        {
            GridVertexData.ClearData();
            Edges.Clear();
        }

        public VertexNode[] GetNeighbors(VertexNode _node)
        {
            return null;
        }
        
        public double GetWeightCost(VertexNode _nodeA, VertexNode _nodeB)
        {
            return 0;
        }

        private void OnDrawGizmos()
        {
            if (!IsDebug) return;
            
            if(Edges == null) return;


            foreach (var _kvp in Edges)
            {
                var _originVertex = _kvp.Key;
                var _neighbors    = _kvp.Value;

                foreach (var _vertex in _neighbors)
                {
                    var _startPos  = _originVertex.NodeWorldPosition;
                    var _targetPos = _vertex.NodeWorldPosition;
            
                    var _dir = _targetPos - _startPos;

                    var _startTangent = _startPos + Vector3.up / 2;
                    var _endTangent   = _startTangent + _dir;

                    
                    if(DisplayBezier)
                        Handles.DrawBezier(_startPos, _targetPos, _startTangent, _endTangent, Color.red, null, EdgeWidth);
                    
                    if(DisplayLine)
                        Gizmos.DrawLine(_startPos, _targetPos);
                }
            }
        }
    }
}
