using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace MugCup_PathFinder.Runtime
{
	[Serializable]
	public class OnDestArrived : UnityEvent
	{
	}
	
    public class PathAgent : MonoBehaviour
    {
	    [SerializeField] private bool followingPath;
	    
	    [SerializeField] private GridNode[] currentFollowedPath;
	    
#region Selected Start/Target Position
	    [SerializeField] private GridNode StartGridNode;
	    [SerializeField] private GridNode TargetGridNode;

	    [field: SerializeField] public Vector3Int StartPosition  { get; private set; }
	    [field: SerializeField] public Vector3Int TargetPosition { get; private set; }
#endregion

	    
#region Dependencies
	    [SerializeField] private GridData<GridNode> GridData;
	    
	    public PathAgent SetGridData(GridData<GridNode> _gridData)
	    {
		    GridData = _gridData;
		    return this;
	    }
	    
	    private IPathFinderController<GridNode> pathFinderController;
	    
	    public PathAgent SetPathFinderController(IPathFinderController<GridNode> _pathFinderController)
	    {
		    pathFinderController = _pathFinderController;
		    pathFinderController.SetGridData(GridData);

		    return this;
	    }
#endregion
	    
	    [Header("Agent Properties")]
	    [SerializeField] private float speed     = 10f;
	    [SerializeField] private float turnSpeed = 5f;
	    
	    
	    [Header("Path Node Tracking Data")]
	    [SerializeField] private Transform model = default;
	    
	    [Space(10)]
	    [SerializeField] private GridNode GridNodeFrom;
	    [SerializeField] private GridNode GridNodeTo;

	    [Space(10)]
	    [SerializeField] private Vector3 positionFrom;
	    [SerializeField] private Vector3 positionTo;

	    [Space(10)]
	    [SerializeField] private NodeDirection   direction;
	    [SerializeField] private DirectionChange directionChange;
	    
	    [Space(10)]
	    [SerializeField] private float directionAngleFrom, directionAngleTo;
	    

	    private Coroutine followPathCoroutine;
	    
	    public OnDestArrived OnDestArrived = new OnDestArrived();

	    public PathAgent SubscribeOnDestArrivedAgent(Action _action)
	    {
		    OnDestArrived?.AddListener(_action.Invoke);
		    return this;
	    }
	    
	    public PathAgent RemoveAllSubscribers()
	    {
		    OnDestArrived?.RemoveAllListeners();
		    return this;
	    }
	    
#region Get Complete Path from outside.
	    //Might not need it or move to on path found
	    public PathAgent SetTargetPath(GridNode[] _targetPath)
	    {
		    currentFollowedPath = _targetPath;
		    return this;
	    }

	    public PathAgent StartFollowPath()
	    {
		    StopFollowPath();
		    followPathCoroutine = StartCoroutine(FollowPath());
		    return this;
	    }

	    public PathAgent StopFollowPath()
	    {
		    if (followPathCoroutine != null)
		    {
			    StopCoroutine(followPathCoroutine);
			    followPathCoroutine = null;
		    } 
		    return this;
	    }
#endregion
	    
#region Calculate Path by agent itself
	    public PathAgent SetStartPos(Vector3Int _startPos)
	    {
		    StartPosition = _startPos;
		    return this;
	    }

	    public PathAgent SetTargetPos(Vector3Int _targetPos)
	    {
		    TargetPosition = _targetPos;
		    return this;
	    }

	    public void RequestPath()
	    {
		    var _newPathRequest = new PathRequest<Vector3Int>(StartPosition, TargetPosition, OnPathFoundHandler);
		    
		    pathFinderController.RequestPath(_newPathRequest);
	    }
	    
	    private void OnPathFoundHandler(Vector3Int[] _wayPoints, bool _pathSuccessful) 
	    {
		    if (!_pathSuccessful)
		    {
			    Debug.LogWarning("Path not found");
			    return;
		    }
		    
		    if(_wayPoints.Length == 0 || _wayPoints.Length == 1) return;

		    var _startNode = _wayPoints[0];
		    var _nextNode  = _wayPoints[1];

		    if (_startNode.x < _nextNode.x)
		    {
			    direction = NodeDirection.East;
		    }
		    else if (_startNode.x > _nextNode.x)
		    {
			    direction = NodeDirection.West;
		    }
		    else if (_startNode.z > _nextNode.z)
		    {
			    direction = NodeDirection.North;
		    }
		    else if (_startNode.z < _nextNode.z)
		    {
			    direction = NodeDirection.South;
		    }
		    
		    SetStartDirection();
			    
		    currentFollowedPath = _wayPoints.Select(_point => GridData.GetNode(_point)).ToArray();
		    followPathCoroutine = StartCoroutine(FollowPathWithNoDirection());
	    }
#endregion

	    private void SetStartDirection()
	    {
		    directionChange = DirectionChange.None;

		    directionAngleFrom = direction.GetAngle();
		    directionAngleTo   = direction.GetAngle();

		    transform.localRotation = direction.GetRotation();
	    }

	    private IEnumerator FollowPathWithNoDirection()
	    {
		    followingPath  = true;
		    
		    int _pathIndex = 0;
		    int _lastIndex = currentFollowedPath.Length - 1;
		    
		    GridNodeTo = currentFollowedPath[_pathIndex];
		    positionTo = GridNodeTo.NodeWorldPosition + Vector3.up;//Temp plus one up

		    while (followingPath)
		    {
			    var _distToNextNode = Vector3.Distance(transform.position, positionTo);
				    
			    if (_distToNextNode > float.Epsilon)
			    {
				    transform.position = Vector3.MoveTowards(transform.position, positionTo, speed * Time.deltaTime);
				    yield return null;
			    }
			    else
			    {
				    _pathIndex++;

				    if (_pathIndex <= _lastIndex)
				    {
					    GridNodeFrom = GridNodeTo;
					    positionFrom = positionTo;
					    
					    GridNodeTo = currentFollowedPath[_pathIndex];

					    positionTo = GridNodeTo.NodeWorldPosition + Vector3.up;//Temp plus one up
				    }
			    }

			    if (_pathIndex > _lastIndex)
			    {
				    StartGridNode = GridNodeTo;
				    StartPosition = GridNodeTo.NodeGridPosition;
				    
				    followingPath = false;
				    
				    OnDestArrived?.Invoke();
			    }
		    }
	    }

	    private IEnumerator FollowPath()
	    {
		    followingPath  = true;
		    
		    int _pathIndex = 0;
		    int _lastIndex = currentFollowedPath.Length - 1;
		    
		    GridNodeTo = currentFollowedPath[_pathIndex];
		    positionTo = GridNodeTo.ExitPosition + Vector3.up;//Temp plus one up

		    while (followingPath)
		    {
			    if (directionChange != DirectionChange.None)
			    {
				    Quaternion _to = Quaternion.Euler(0, directionAngleTo,   0);
				    transform.localRotation = Quaternion.RotateTowards(transform.localRotation, _to, turnSpeed * Time.deltaTime);
			    
				    yield return null;
			    
				    if (transform.localRotation == _to)
				    {
					    _pathIndex++;
					    
					    if(_pathIndex <= _lastIndex) 
						    SetNextNode(_pathIndex);
				    }
			    }
			    else
			    {
				    var _distToNextNode = Vector3.Distance(transform.position, positionTo);
				    
				    if (_distToNextNode > float.Epsilon)
				    {
					    transform.position = Vector3.MoveTowards(transform.position, positionTo, speed * Time.deltaTime);
					    yield return null;
				    }
				    else
				    {
					    _pathIndex++;
					    
					    if(_pathIndex <= _lastIndex)
							SetNextNode(_pathIndex);
				    }
			    }

			    if (_pathIndex > _lastIndex)
			    {
				    StartGridNode = GridNodeTo;
				    StartPosition = GridNodeTo.NodeGridPosition;
				    
				    followingPath = false;
				    
				    OnDestArrived?.Invoke();
			    }
		    }
	    }

	    private void SetNextNode(int _index)
	    {
		    GridNodeFrom = GridNodeTo;
		    positionFrom = positionTo;
					    
		    GridNodeTo = currentFollowedPath[_index];

		    if (_index == currentFollowedPath.Length - 1)
		    {
			    positionTo = GridNodeTo.NodeWorldPosition + Vector3.up;//Temp plus one up
		    }
		    else
		    {
				positionTo = GridNodeTo.ExitPosition + Vector3.up;//Temp plus one up
		    }

		    directionChange = direction.GetDirectionChangeTo(GridNodeTo.Direction);
		    direction       = GridNodeTo.Direction;
					    
		    directionAngleFrom = directionAngleTo;
					    
		    switch (directionChange) 
		    {
			    case DirectionChange.None     : PrepareForward()   ; break;
			    case DirectionChange.TurnRight: PrepareTurnRight() ; break;
			    case DirectionChange.TurnLeft : PrepareTurnLeft()  ; break;
			    default:                        PrepareTurnAround(); break;
		    }
	    }
	    
	    private void PrepareForward () 
	    {
		    directionAngleTo        = direction.GetAngle();
		    transform.localRotation = direction.GetRotation();
		    
		    model.localPosition = Vector3.zero;
		    transform.localPosition = positionFrom;
	    }

	    private void PrepareTurnRight () 
	    {
		    directionAngleTo = directionAngleFrom + 90f;
		    
		    model.localPosition     = new Vector3(-0.5f, 0f);
		    transform.localPosition = positionFrom + direction.GetHalfVector();
	    }

	    private void PrepareTurnLeft () 
	    {
		    directionAngleTo = directionAngleFrom - 90f;
		    
		    model.localPosition     = new Vector3(0.5f, 0f);
		    transform.localPosition = positionFrom + direction.GetHalfVector();
	    }

	    private void PrepareTurnAround ()
	    {
		    directionAngleTo = directionAngleFrom + 180f;
		    
		    model.localPosition     = Vector3.zero;
		    transform.localPosition = positionFrom;
	    }

	    private void OnDrawGizmos()
	    {
		    if (currentFollowedPath is { Length: > 0 })
		    {
			    for(var _i = 0; _i < currentFollowedPath.Length - 1; _i++)
			    {
				    var _node     = currentFollowedPath[_i];
				    var _nextNode = currentFollowedPath[_i + 1];

				    var _nodePos     = _node.NodeWorldPosition + Vector3.up;
				    var _nextNodePos = _nextNode.NodeWorldPosition + Vector3.up;
				    
				    Gizmos.color = Color.red;
				    Gizmos.DrawLine(_nodePos, _nextNodePos);
				    
				    Gizmos.color = Color.green;
				    Gizmos.DrawSphere(_nodePos, 0.1f);

				    if (_i == currentFollowedPath.Length - 2)
					    Gizmos.DrawSphere(_nextNodePos, 0.1f);
			    }
		    }
	    }
    }
}
