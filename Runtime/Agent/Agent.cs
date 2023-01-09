using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class Agent : MonoBehaviour
    {
	    [SerializeField] private bool useGridNodeDataManager;
	    [SerializeField] private bool useNodeAsPosition;
	    [SerializeField] private bool followingPath;
	    
	    [SerializeField] private float speed     = 10f;
	    [SerializeField] private float turnSpeed = 5f;

	    [SerializeField] private GridNode[] currentFollowedPath;
	    
#region Selected Start/Target Position
	    [SerializeField] private GridNode StartGridNode;
	    [SerializeField] private GridNode TargetGridNode;

	    [SerializeField] private Vector3Int startPosition ;
	    [SerializeField] private Vector3Int targetPosition;
#endregion

#region Dependencies
	    [SerializeField] private GridNodeDataManager    gridNodeDataManager;
	    [SerializeField] private GridData<GridNode> GridData;
	    
	    private IPathFinderController<GridNode> pathFinderController;
#endregion

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

	    public void SetUseGridNodeDataManager(bool _value) => useGridNodeDataManager = _value;
	    public void SetUseNodeAsPosition     (bool _value) => useNodeAsPosition      = _value;
	    
	    public void LoadGridData(GridData<GridNode> _gridData) => GridData = _gridData;

	    public void Initialized(IPathFinderController<GridNode> _pathFinderController = null)
	    {
		    if(useGridNodeDataManager)
				InjectGridNodeDataManager();
		    else
			    InjectCustomGridNodeData(GridData);
		    
		    InjectPathFinderController(_pathFinderController);
	    }
	    
#region Inject Dependencies
	    /// <summary>
	    /// Use GridNodeData as Default Initialization.
	    /// </summary>
	    private void InjectGridNodeDataManager(GridNodeDataManager _gridNodeDataManager = null)
	    {
		    if(gridNodeDataManager == null)
				gridNodeDataManager = _gridNodeDataManager != null ? _gridNodeDataManager : FindObjectOfType<GridNodeDataManager>();

		    if (!gridNodeDataManager)
		    {
			    Debug.LogWarning($"GridNodeData Missing Reference.");
			    return;
		    }

		    GridData = gridNodeDataManager.GetGridNodeData();
	    }
	    
	    private void InjectCustomGridNodeData(GridData<GridNode> _gridData)
	    {
		    GridData = _gridData;
	    }
	    
	    public void InjectPathFinderController(IPathFinderController<GridNode> _pathFinderController = null)
	    {
		    pathFinderController = _pathFinderController ?? FindObjectOfType<PathFinderControllerNodeBase>();

		    if (pathFinderController == null)
		    {
			    Debug.LogWarning($"{typeof(IPathFinderController<GridNode>)} Missing Reference.");
		    }
	    }
  #endregion

	    /// <summary>
	    /// Get Complete Path from outside.
	    /// </summary>
	    /// <param name="_targetPath"></param>
	    public Agent SetTargetPath(GridNode[] _targetPath)
	    {
		    currentFollowedPath = _targetPath;
		    return this;
	    }

	    public Agent StartFollowPath()
	    {
		    StopFollowPath();
		    followPathCoroutine = StartCoroutine(FollowPath());
		    return this;
	    }

	    public Agent StopFollowPath()
	    {
		    if (followPathCoroutine == null) 
			    return this;
		    
		    StopCoroutine(followPathCoroutine);
		    followPathCoroutine = null;

		    return this;
	    }

	    /// <summary>
	    /// Calculate Path by agent itself
	    /// </summary>
	    public Agent SetStartPos(Vector3Int _startPos)
	    {
		    startPosition = _startPos;
		    return this;
	    }

	    public Agent SetTargetPos(Vector3Int _targetPos)
	    {
		    targetPosition = _targetPos;
		    return this;
	    }

	    public void StartFindPath()
	    {
		    if (followPathCoroutine != null)
		    {
			    StopCoroutine(followPathCoroutine);
			    followPathCoroutine = null;
		    }

		    if (!useNodeAsPosition)
		    {
			    StartGridNode  = GridUtility.GetNode(startPosition,  GridData);
			    TargetGridNode = GridUtility.GetNode(targetPosition, GridData);
		    }
		    
		    var _newPathRequest = new PathRequestNodeBase(StartGridNode, TargetGridNode, OnPathFoundHandler);
		    
		    pathFinderController.RequestPath(_newPathRequest);
	    }
	    
	    private void OnPathFoundHandler(GridNode[] _nodePath, bool _pathSuccessful) 
	    {
		    if (!_pathSuccessful) return;
			    
		    currentFollowedPath = _nodePath;
		    followPathCoroutine = StartCoroutine(FollowPath());
	    }

	    private IEnumerator FollowPath()
	    {
		    direction       = NodeDirection.East;
		    directionChange = DirectionChange.None;

		    directionAngleFrom = direction.GetAngle();
		    directionAngleTo   = direction.GetAngle();

		    transform.localRotation = direction.GetRotation();
		    
		    followingPath  = true;
		    
		    int _pathIndex = 0;
		    int _lastIndex = currentFollowedPath.Length - 1;

		    while (followingPath)
		    {
			    GridNodeTo = currentFollowedPath[_pathIndex];
			    positionTo = GridNodeTo.ExitPosition + Vector3.up;//Temp plus one up
			  
			    if (directionChange != DirectionChange.None)
			    {
				    Quaternion _to = Quaternion.Euler(0, directionAngleTo,   0);
				    transform.localRotation = Quaternion.RotateTowards(transform.localRotation, _to, turnSpeed * Time.deltaTime);

				    yield return null;

				    if (transform.localRotation == _to)
				    {
					    _pathIndex++;
				    
					    GridNodeFrom     = GridNodeTo;
					    positionFrom = positionTo;
				    
					    GridNodeTo     = currentFollowedPath[_pathIndex];
					    positionTo = GridNodeTo.ExitPosition + Vector3.up;//Temp plus one up

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
					    
					    GridNodeFrom     = GridNodeTo;
					    positionFrom = positionTo;
					    
					    GridNodeTo     = currentFollowedPath[_pathIndex];
					    positionTo = GridNodeTo.ExitPosition + Vector3.up;//Temp plus one up

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
			    }
			    

			    if (_pathIndex >= _lastIndex)
				    followingPath = false;
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

	    public IEnumerator FollowPath(PathInfo _pathInfo)
	    {
		    bool _followingPath = true;
		    
		    int _pathIndex = 0;
		    
		    transform.LookAt(_pathInfo.LookPoints[0]);

		    float _speedPercent = 1;

		    while (_followingPath) 
		    {
			    Vector2 pos2D = new Vector2 (transform.position.x, transform.position.z);
			    
			    //while (_pathInfo.turnBoundaries [pathIndex].HasCrossedLine (pos2D)) {
				//    if (pathIndex == _pathInfo.finishLineIndex) {
				//	    followingPath = false;
				//	    break;
				//    } else {
				//	    pathIndex++;
				//    }
			    //}
			    //
			    //if (followingPath) {
			    //
				//    if (pathIndex >= _pathInfo.slowDownIndex && stoppingDst > 0) {
				//	    speedPercent = Mathf.Clamp01 (_pathInfo.turnBoundaries [_pathInfo.finishLineIndex].DistanceFromPoint (pos2D) / stoppingDst);
				//	    if (speedPercent < 0.01f) {
				//		    followingPath = false;
				//	    }
				//    }
			    //
				//    Quaternion targetRotation = Quaternion.LookRotation (_pathInfo.lookPoints [pathIndex] - transform.position);
				//    transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
				//    transform.Translate (Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
			    //}

			    yield return null;
		    }
	    }
    }
}
