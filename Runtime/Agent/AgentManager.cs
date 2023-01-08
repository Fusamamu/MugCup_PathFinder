using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MugCup_PathFinder.Runtime
{
    public class AgentManager : MonoBehaviour
    {
        [SerializeField] private Agent AgentPrefab;
        
        public int AgentCount = 10;
        public float AgentSpeed = 1f;
        
        private List<Agent> Agents = new List<Agent>();

        [SerializeField] private GridVertexData GridVertexData;

        public void PopulateAgents()
        {
            PopulateAgents(GridVertexData);
        }

        public void ClearAgents()
        {
            foreach (var _agent in Agents)
                Destroy(_agent.gameObject);
            
            Agents.Clear();
        }

        public void PopulateAgents<T>(GridData<T> _data) where T : INode
        {
            for (var _i = 0; _i < AgentCount; _i++)
            {
                var _index = Random.Range(0, _data.GridNodes.Length);
                var _node  = _data.GridNodes[_index];
                
                if(_node == null) continue;
                
                var _newAgent = Instantiate(AgentPrefab, _node.NodeWorldPosition, Quaternion.identity);

                Agents.Add(_newAgent);

                var _movement = _newAgent.gameObject.AddComponent<AgentMovement>();
                
                _movement.SetCurrentNode(_node);
                _movement.SetMoveSpeed(AgentSpeed);

                if (_node.NodeParent != null)
                {
                    _movement.SetNextNode(_node.NodeParent);
                    _movement.MoveToNextNode();
                }

                // var _followFlow = _newAgent.gameObject.AddComponent<FollowFlowField>();
                //
                // _followFlow.SetCurrentNode(_checkNode);
                // _followFlow.SetMoveSpeed(AgentSpeed);
                //
                // if (_checkNode.NodeParent != null)
                // {
                //     _followFlow.SetNextNode(_checkNode.NodeParent as GridNode);
                //     _followFlow.MoveToNextNode();
                // }
            }
        }
    }
}
