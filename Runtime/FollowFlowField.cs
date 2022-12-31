using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class FollowFlowField : MonoBehaviour
    {
        [field: SerializeField] public GridNode CurrentGridNode { get; private set; }
        [field: SerializeField] public GridNode NextGridNode    { get; private set; }

        [SerializeField] private float MoveSpeed = 1f;

        private Coroutine moveProcess;

        public void SetMoveSpeed(float _speed)
        {
            MoveSpeed = _speed;
        }

        public void SetCurrentNode(GridNode _gridNode)
        {
            CurrentGridNode = _gridNode;
        }

        public void SetNextNode(GridNode _gridNode)
        {
            NextGridNode = _gridNode;
        }

        public void MoveToNextNode()
        {
            moveProcess = StartCoroutine(MoveCoroutine());
        }

        public IEnumerator MoveCoroutine()
        {
            var _originPos  = CurrentGridNode.NodeWorldPosition  + Vector3.up / 2;
            var _targetPos  = NextGridNode   .NodeWorldPosition  + Vector3.up / 2;

            float _tValue = 0;

            while(Vector3.Distance(transform.position, _targetPos) > float.Epsilon)
            {
                _tValue += Time.deltaTime * MoveSpeed;
                _tValue = Mathf.Clamp01(_tValue);
                
                transform.position = Vector3.Lerp(_originPos, _targetPos, _tValue);
            
                yield return null;

                if (Vector3.Distance(transform.position, _targetPos) <= float.Epsilon)
                {
                    if(NextGridNode.NodeParent == null)
                        yield break;
                    
                    SetCurrentNode(NextGridNode);

                    if (CurrentGridNode.NodeParent != null)
                    {
                        SetNextNode(CurrentGridNode.NodeParent as GridNode);

                        _originPos = CurrentGridNode.NodeWorldPosition + Vector3.up / 2;
                        _targetPos = NextGridNode.NodeWorldPosition    + Vector3.up / 2;

                        _tValue = 0;
                    }
                    else
                        yield break;
                    
                }
            }
        }
    }
}
