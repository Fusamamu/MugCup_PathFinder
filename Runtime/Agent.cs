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

	    [SerializeField] private NodeBase[] currentFollowedPath;
	    
#region Selected Start/Target Position
	    [SerializeField] private NodeBase startNode;
	    [SerializeField] private NodeBase targetNode;

	    [SerializeField] private Vector3Int startPosition ;
	    [SerializeField] private Vector3Int targetPosition;
#endregion

#region Dependencies
	    [SerializeField] private GridNodeDataManager    gridNodeDataManager;
	    [SerializeField] private GridNodeData<NodeBase> gridNodeData;
	    
	    private IPathFinderController<NodeBase> pathFinderController;
#endregion

	    private Coroutine followPathCoroutine;

	    public void SetUseGridNodeDataManager(bool _value) => useGridNodeDataManager = _value;
	    public void SetUseNodeAsPosition     (bool _value) => useNodeAsPosition      = _value;
	    
	    public void LoadGridData(GridNodeData<NodeBase> _gridNodeData) => gridNodeData = _gridNodeData;

	    public void Initialized(IPathFinderController<NodeBase> _pathFinderController = null)
	    {
		    if(useGridNodeDataManager)
				InjectGridNodeDataManager();
		    else
			    InjectCustomGridNodeData(gridNodeData);
		    
		    InjectPathFinderController(_pathFinderController);
	    }
	    
#region Inject Dependencies
	    /// <summary>
	    /// Use GridNodeData as Default Initialization.
	    /// </summary>
	    /// <param name="_gridNodeDataManager"></param>
	    private void InjectGridNodeDataManager(GridNodeDataManager _gridNodeDataManager = null)
	    {
		    if(gridNodeDataManager == null)
				gridNodeDataManager = _gridNodeDataManager != null ? _gridNodeDataManager : FindObjectOfType<GridNodeDataManager>();

		    if (!gridNodeDataManager)
		    {
			    Debug.LogWarning($"GridNodeData Missing Reference.");
			    return;
		    }

		    gridNodeData = gridNodeDataManager.GetGridNodeData();
	    }
	    
	    private void InjectCustomGridNodeData(GridNodeData<NodeBase> _gridNodeData)
	    {
		    gridNodeData = _gridNodeData;
	    }
	    
	    public void InjectPathFinderController(IPathFinderController<NodeBase> _pathFinderController = null)
	    {
		    pathFinderController = _pathFinderController ?? FindObjectOfType<PathFinderControllerNodeBase>();

		    if (pathFinderController == null)
		    {
			    Debug.LogWarning($"{typeof(IPathFinderController<NodeBase>)} Missing Reference.");
		    }
	    }
  #endregion

	    /// <summary>
	    /// Get Complete Path from outside.
	    /// </summary>
	    /// <param name="_targetPath"></param>
	    public Agent SetTargetPath(NodeBase[] _targetPath)
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

	    // public Agent SetPathToFollow(NodeBase[] _path)
	    // {
		   //  currentFollowedPath = _path;
		   //  return this;
	    // }

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
	    /// <param name="_startPos"></param>
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
			    startNode  = GridUtility.GetNode(startPosition,  gridNodeData);
			    targetNode = GridUtility.GetNode(targetPosition, gridNodeData);
		    }
		    
		    var _newPathRequest = new PathRequestNodeBase(startNode, targetNode, OnPathFoundHandler);
		    
		    pathFinderController.RequestPath(_newPathRequest);
	    }
	    
	    private void OnPathFoundHandler(NodeBase[] _nodePath, bool _pathSuccessful) 
	    {
		    if (!_pathSuccessful) return;
			    
		    currentFollowedPath = _nodePath;
		    followPathCoroutine = StartCoroutine(FollowPath());
	    }

	    private IEnumerator FollowPath()
	    { 
		    followingPath  = true;
		    
		    int _pathIndex = 0;
		    int _lastIndex = currentFollowedPath.Length - 1;

		    while (followingPath)
		    {
			    var _currentCellPos = (Vector3)currentFollowedPath[_pathIndex].NodePosition + Vector3.up;//Temp plus one up
			    var _distToNextNode = Vector3.Distance(transform.position, _currentCellPos);
			    
			    if (_distToNextNode > float.Epsilon)
			    {
				    transform.position = Vector3.MoveTowards(transform.position, _currentCellPos, speed * Time.deltaTime);
				    yield return null;
			    }
			    else
			    {
				    _pathIndex++;
			    }

			    if (_pathIndex >= _lastIndex)
				    followingPath = false;
		    }
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
