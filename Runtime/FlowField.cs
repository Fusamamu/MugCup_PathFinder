using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Graphs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MugCup_PathFinder.Runtime
{
    public class FlowField : MonoBehaviour
    {
        [SerializeField] private Agent AgentPrefab;
        public int AgentCount = 10;
        public float AgentSpeed = 1f;
        private List<Agent> agents = new List<Agent>();
        
        [SerializeField] private Transform GridParent;
        [SerializeField] private GridNode GridNodePrefab;
        [SerializeField] private GridNodeData GridNodeData;

        public Vector3Int SourceNodePos;

        public Color DefaultColor;
        public Color StructureColor;
        public Color CurrentNodeColor;
        public Color ActiveNeighborColor;
        public Color NeighborColor;
        public Color OpenColor;
        public Color CloseColor;

        public Gradient HeatColor;
        public float Divider;
        

        public float AnimationInterval = 1f;
        private Coroutine searchProcess;

        private static MaterialPropertyBlock materialPropertyBlock;
        private static readonly int baseColor = Shader.PropertyToID("_BaseColor");

        private void OnValidate()
        {
            foreach (var _node in GridNodeData.GridNodes)
            {
                if(_node.gameObject.layer != LayerMask.NameToLayer("Structure")) continue;
                
                SetNodeColor(_node, StructureColor);
                
                _node.SetNodeWorldPosition(_node.transform.position);
            }
        }

        private void Start()
        {
            //StartSearch();
        }

        public void UpdateStructureNodes()
        {
            foreach (var _node in GridNodeData.GridNodes)
            {
                if(_node.gameObject.layer != LayerMask.NameToLayer("Structure")) continue;

                _node.transform.position -= Vector3.up * 2;
                
                materialPropertyBlock ??= new MaterialPropertyBlock();
                materialPropertyBlock.SetColor(baseColor, StructureColor);
                _node.GetComponent<MeshRenderer>().SetPropertyBlock(materialPropertyBlock);
            }
        }

        public void UpdateNodeWorldPosition()
        {
            foreach (var _node in FindObjectsOfType<GridNode>())
                _node.SetNodeWorldPosition(_node.transform.position);
        }

        public void GenerateGrid()
        {
            if (!GridNodePrefab)
            {
                Debug.LogError("Need Node Prefab in Flow Filed");
                return;
            }
            
            GridNodeData.GridNodes = GridUtility.GenerateGridINodes<GridNode>(GridNodeData.GridSize, GridNodePrefab.gameObject, GridParent.gameObject);

            foreach (var _node in GridNodeData.GridNodes)
            {
                // if(Random.Range(0, 5) == 1)
                // {
                //     int _structureLayer = LayerMask.NameToLayer("Structure");
                //     _node.gameObject.layer = _structureLayer;
                //     
                //     var _block = new MaterialPropertyBlock();
                //     _block.SetColor(baseColor, StructureColor);
                //     _node.GetComponent<MeshRenderer>().SetPropertyBlock(_block);
                // }
                
                _node.transform.SetParent(GridParent);
            }
        }

        public void ClearGrid()
        {
            foreach (var _node in GridNodeData.GridNodes)
            {
                if (!Application.isPlaying)
                    DestroyImmediate(_node.gameObject);
                else
                    Destroy(_node.gameObject);
            }

            GridNodeData.GridNodes = null;
        }

        public void StartSearch()
        {
            searchProcess = StartCoroutine(BreadthFirstSearch());
            
            foreach (var _node in GridNodeData.GridNodes)
            {
                if(_node.gameObject.layer != LayerMask.NameToLayer("Structure")) continue;

                _node.transform.position += Vector3.up * 2;
                
                materialPropertyBlock ??= new MaterialPropertyBlock();
                materialPropertyBlock.SetColor(baseColor, StructureColor);
                _node.GetComponent<MeshRenderer>().SetPropertyBlock(materialPropertyBlock);
            }
            
            //BreadthFirstSearch();
        }

        public void StopSearch()
        {
            if (searchProcess != null)
            {
                StopCoroutine(searchProcess);
                searchProcess = null;
            }
        }

        public void Reset()
        {
            StopSearch();
            ClearGrid();
            GenerateGrid();
        }

        public IEnumerator BreadthFirstSearch()
        {
            var _openSet       = new Queue<GridNode>();
            var _closeSet      = new HashSet<GridNode>();
            
            var _structureNode = new HashSet<GridNode>();
            var _distTable     = new Dictionary<GridNode, float>();

            var _startNode = GridUtility.GetNode(SourceNodePos, GridNodeData);
            _startNode.transform.position += Vector3.up;
            _startNode.NodeParent = null;
            _distTable.Add(_startNode, 0);
            
            _openSet.Enqueue(_startNode);
            _closeSet.Add(_startNode);

            while (_openSet.Count > 0)
            {
                foreach (var _node in _openSet)
                    SetNodeColor(_node, OpenColor);
                
                var _currentNode = _openSet.Dequeue();
                
                SetNodeColor(_currentNode, CurrentNodeColor);

                var _neighbors = GridUtility
                    .GetAdjacentNodes8Dir(_currentNode, GridNodeData.GridSize, GridNodeData.GridNodes)
                    .Where(_node => _node != null);
                
                foreach (var _node in _neighbors)
                {
                    if (_node.gameObject.layer == LayerMask.NameToLayer("Structure"))
                        continue;
                    
                    SetNodeColor(_node, NeighborColor);
                }

                GridNode _activeNeighbor = null;

                foreach (var _node in _neighbors)
                {
                    if (_node.gameObject.layer == LayerMask.NameToLayer("Structure") && !_closeSet.Contains(_node))
                    {
                        if(_structureNode.Contains(_node)) continue;
                        _structureNode.Add(_node);
                        
                        //_node.transform.position += Vector3.up * 2;
                        continue;
                    }

                    if (!_distTable.ContainsKey(_node))
                    {
                        var _distValue = _distTable[_currentNode] + 1;
                        _distTable.Add(_node, _distValue);
                    }

                    if (_node.NodeParent == null)
                        _node.NodeParent = _currentNode;
                    
                    if (_activeNeighbor != null)
                        SetNodeColor(_activeNeighbor, NeighborColor);

                    _activeNeighbor = _node;
                    SetNodeColor(_activeNeighbor, ActiveNeighborColor);
                    
                    
                    if (!_closeSet.Contains(_node))
                    {
                        _openSet.Enqueue(_node);
                        _closeSet.Add(_node);

                        _node.transform.position += Vector3.up;
                        SetNodeColor(_node, CloseColor);

                        yield return new WaitForSeconds(AnimationInterval);
                    }
                }

                foreach (var _node in _closeSet)
                {
                    if (_distTable.TryGetValue(_node, out var _value))
                        SetNodeColor(_node, _value);
                }
                
                Debug.Log("Still in loop");
            }
        }

        public void PopulateAgents()
        {
            for (var _i = 0; _i < AgentCount; _i++)
            {
                var _xRandom = Random.Range(0, GridNodeData.GridSize.x);
                var _zRandom = Random.Range(0, GridNodeData.GridSize.z);

                var _targetPos = new Vector3Int(_xRandom, 0, _zRandom);

                var _checkNode = GridUtility.GetNode(_targetPos, GridNodeData.GridSize, GridNodeData.GridNodes);
                if (_checkNode != null && _checkNode.gameObject.layer != LayerMask.NameToLayer("Structure"))
                {
                    var _newAgent = Instantiate(AgentPrefab, _targetPos + Vector3.up/2, Quaternion.identity);
                    agents.Add(_newAgent);
                    
                    var _followFlow = _newAgent.gameObject.AddComponent<FollowFlowField>();
                    
                    _followFlow.SetCurrentNode(_checkNode);
                    _followFlow.SetMoveSpeed(AgentSpeed);

                    if (_checkNode.NodeParent != null)
                    {
                        _followFlow.SetNextNode(_checkNode.NodeParent as GridNode);
                        _followFlow.MoveToNextNode();
                    }
                }
            }
        }

        public void MoveAgents()
        {
            foreach (var _agent in agents)
            {
                var _agentPos = _agent.transform.position;
                var _gridPos = new Vector3Int((int)_agentPos.x, 0, (int)_agentPos.z);
                
                var _currentNode = GridUtility.GetNode(_gridPos, GridNodeData.GridSize, GridNodeData.GridNodes);

                if (_currentNode.NodeParent != null)
                {
                    StartCoroutine(MoveAgent(_agent, _agent.transform.position, _currentNode.NodeParent.NodeWorldPosition));
                }
            }
        }

        private IEnumerator MoveAgent(Agent _agent, Vector3 _from , Vector3 _to)
        {
            var _totalDist = Vector3.Distance(_from, _to);


            float  _t = 0;
            
            while (Vector3.Distance(_agent.transform.position, _to) > 0.01f)
            {
                var _currentDist = Vector3.Distance(_agent.transform.position, _to);

                _t += AgentSpeed * Time.deltaTime;
                
                _agent.transform.position = Vector3.Lerp(_from, _to, _t);
                yield return null;
            }
            
            var _agentPos = _agent.transform.position;
            var _gridPos = new Vector3Int((int)_agentPos.x, 0, (int)_agentPos.z);
                
            var _currentNode = GridUtility.GetNode(_gridPos, GridNodeData.GridSize, GridNodeData.GridNodes);
            
            if (_currentNode.NodeParent != null)
            {
                StartCoroutine(MoveAgent(_agent, _agent.transform.position, _currentNode.NodeParent.NodeWorldPosition));
            }
        }

        private void SetNodeColor(GridNode _gridNode, float _value)
        {
            //return;
            var _heatColor = HeatColor.Evaluate(_value / Divider);
            SetNodeColor(_gridNode, _heatColor);
        }
        
        private void SetNodeColor(GridNode _gridNode, Color _color)
        {
            //return;
            materialPropertyBlock ??= new MaterialPropertyBlock();
            materialPropertyBlock.SetColor(baseColor, _color);
            _gridNode.GetComponent<MeshRenderer>().SetPropertyBlock(materialPropertyBlock);
        }
        
    }
}
