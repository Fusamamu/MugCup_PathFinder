using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class AgentMovement : MonoBehaviour
    {
        [field: SerializeField] public INode CurrentNode { get; private set; }
        [field: SerializeField] public INode NextNode    { get; private set; }

        [SerializeField] private float MoveSpeed = 1f;

        private Coroutine moveProcess;

        public void SetMoveSpeed(float _speed)
        {
            MoveSpeed = _speed;
        }

        public void SetCurrentNode(INode _gridNode)
        {
            CurrentNode = _gridNode;
        }

        public void SetNextNode(INode _gridNode)
        {
            NextNode = _gridNode;
        }

        public void MoveToNextNode()
        {
            moveProcess = StartCoroutine(MoveCoroutine());
        }

        public IEnumerator MoveCoroutine()
        {
            var _originPos = CurrentNode.NodeWorldPosition;
            var _targetPos = NextNode.NodeWorldPosition;

            float _tValue = 0;

            while(Vector3.Distance(transform.position, _targetPos) > float.Epsilon)
            {
                _tValue += Time.deltaTime * MoveSpeed;
                _tValue = Mathf.Clamp01(_tValue);
                
                transform.position = Vector3.Lerp(_originPos, _targetPos, _tValue);
            
                yield return null;

                if (Vector3.Distance(transform.position, _targetPos) <= float.Epsilon)
                {
                    if(NextNode.NodeParent == null)
                        yield break;
                    
                    SetCurrentNode(NextNode);

                    if (CurrentNode.NodeParent != null)
                    {
                        SetNextNode(CurrentNode.NodeParent);

                        _originPos = CurrentNode.NodeWorldPosition;
                        _targetPos = NextNode.NodeWorldPosition;

                        _tValue = 0;
                    }
                    else
                        yield break;
                }
            }
        }
    }
}
