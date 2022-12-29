using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class FollowFlowField : MonoBehaviour
    {
        [field: SerializeField] public NodeBase CurrentNode { get; private set; }
        [field: SerializeField] public NodeBase NextNode    { get; private set; }

        [SerializeField] private float MoveSpeed = 1f;

        private Coroutine moveProcess;

        public void SetMoveSpeed(float _speed)
        {
            MoveSpeed = _speed;
        }

        public void SetCurrentNode(NodeBase _node)
        {
            CurrentNode = _node;
        }

        public void SetNextNode(NodeBase _node)
        {
            NextNode = _node;
        }

        public void MoveToNextNode()
        {
            moveProcess = StartCoroutine(MoveCoroutine());
        }

        public IEnumerator MoveCoroutine()
        {
            var _originPos  = CurrentNode.NodeWorldPosition  + Vector3.up / 2;
            var _targetPos  = NextNode   .NodeWorldPosition  + Vector3.up / 2;

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
                        SetNextNode(CurrentNode.NodeParent as NodeBase);

                        _originPos = CurrentNode.NodeWorldPosition + Vector3.up / 2;
                        _targetPos = NextNode.NodeWorldPosition    + Vector3.up / 2;

                        _tValue = 0;
                    }
                    else
                        yield break;
                    
                }
            }
        }
    }
}
