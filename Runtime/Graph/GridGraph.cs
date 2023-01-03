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
        [Header("Grid Data")]
        [SerializeField] private GridNodeData   GridData;
        [SerializeField] private GridVertexData GridVertexData;

        [field: SerializeField] public List<GraphEdge> GraphEdges { get; private set; }   = new List<GraphEdge>();
        
        public Dictionary<VertexNode, VertexNode[]> Edges { get; private set; }

        public Dictionary<VertexNode, GridNode> VertexToGirdNodeTable { get; private set; } = new Dictionary<VertexNode, GridNode>();
        
        

        [Header("Game Object")]
        [SerializeField] private VertexNode VertexPrefab;
        [SerializeField] private Transform VertexParent;

        [Header("Debug")]
        [SerializeField] private bool IsDebug;
        [SerializeField] private bool DisplayBezier;
        [SerializeField] private bool DisplayLine;
        [SerializeField] private float EdgeWidth;
        

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
                MapGraph();
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
            
            
            
            foreach (var _node in GridData.GridNodes.Where(_node => _node != null))
            {
                if (!GridUtility.HasNodeOnTop(_node, GridData.GridSize, GridData.GridNodes))
                {
                    AddVertex(_node);
                }
                else
                {
                    var _topNode = GridUtility.GetNodeUp(_node.NodeGridPosition, GridData.GridSize, GridData.GridNodes);
                    if (!_topNode.IsObstacle)
                    {
                        AddVertex(_node);
                    }
                }
            }
            
            
            
            
            
            
           
            MapGraph();

            //Must move to somewhereelse
            #if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorUtility.SetDirty(GridVertexData);
            #endif
        }

        private void AddVertex(GridNode _node)
        {
            if(_node.IsVertexNodeInit) return;
            _node.SetVertexNodeInit(true);

            foreach ((Vector3 _vertexWorldPos, Vector3Int _vertexGridPos) in _node.GenerateVertexNodePositions())
            {
                var _vertexNode = AddVertex(_vertexGridPos, _vertexWorldPos);
                
                if (!VertexToGirdNodeTable.ContainsKey(_vertexNode))
                    VertexToGirdNodeTable.Add(_vertexNode, _node);
            }
        }

        private VertexNode AddVertex(Vector3Int _atGridPos, Vector3 _atWorldPosition)
        {
            var _vertexNode = Instantiate(VertexPrefab, _atWorldPosition, Quaternion.identity, VertexParent);
                        
            _vertexNode.SetNodeWorldPosition(_atWorldPosition);
            _vertexNode.SetNodePosition     (_atGridPos);

            GridVertexData.AddNode(_vertexNode, _atGridPos);

            return _vertexNode;
        }

        private void AddEdge(VertexNode _from, VertexNode _to)
        {
            var _edge = new GraphEdge(_from, _to);
            GraphEdges.Add(_edge);
        }

        private void MapGraphRevised()
        {
            
            foreach (var _vertex in GridVertexData.GridNodes.Where(_vertex => _vertex != null))
            {
                var _vertices = GetVertexNeighbors(_vertex);

                if (VertexToGirdNodeTable.TryGetValue(_vertex, out var _gridNode))
                {
                    //Should find a way to set this beforehand
                    if (_gridNode.ConnectorCount == 0)
                        _gridNode.Connect4Directions();
                        
                    var _validVertexConnects = _gridNode
                        .WorldNodeConnector
                        .Select(_connectPos => _connectPos + Vector3.up)
                        .Select(_vertexPos =>
                        {
                            var _vertexGridPos = new Vector3Int((int)_vertexPos.x, (int)_vertexPos.y, (int)_vertexPos.z);
                            return _vertexGridPos;
                        });

                    _vertices = _vertices.Where(_v => _validVertexConnects.Contains(_v.NodeGridPosition)).ToArray();
                }
                    
            }
        }

        public IEnumerable<VertexNode> GetVertexNeighbors(VertexNode _vertex)
        {
            return GridUtility
                .GetNodesFrom3x3Cubes(_vertex, GridVertexData.GridSize, GridVertexData.GridNodes)
                .Where(_v => _v != null);
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
                        .GetNodesFrom3x3Cubes(_vertex, GridVertexData.GridSize, GridVertexData.GridNodes)
                        .Where(_v => _v != null)
                        .ToArray();

                    if (VertexToGirdNodeTable.TryGetValue(_vertex, out var _gridNode))
                    {
                        if (_gridNode.ConnectorCount == 0)
                            _gridNode.Connect4Directions();
                        
                        var _validVertexConnects = _gridNode
                            .WorldNodeConnector
                            .Select(_connectPos => _connectPos + Vector3.up)
                            .Select(_vertexPos =>
                            {
                                var _vertexGridPos = new Vector3Int((int)_vertexPos.x, (int)_vertexPos.y, (int)_vertexPos.z);
                                return _vertexGridPos;
                            });

                        _vertices = _vertices.Where(_v => _validVertexConnects.Contains(_v.NodeGridPosition)).ToArray();
                    }
                    
                    Edges.Add(_vertex, _vertices);
                }
            }
        }

        
        
        
        
        public void ClearVertexData()
        {
            GridVertexData.ClearData();
            Edges.Clear();
        }

        // public VertexNode[] GetNeighbors(VertexNode _node)
        // {
        //     return null;
        // }
        //
        // public double GetWeightCost(VertexNode _nodeA, VertexNode _nodeB)
        // {
        //     return 0;
        // }

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
