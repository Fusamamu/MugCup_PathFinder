using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class Agent : MonoBehaviour
    {
	    [SerializeField] private float speed     = 0.05f;
	    [SerializeField] private float turnSpeed = 5f;

	    [SerializeField] private NodeBase[] currentFollowedPath;

	    [SerializeField] private bool useNode;
	    [SerializeField] private bool followingPath;

	    
#region Selected Start/Target Position
	    [SerializeField] private NodeBase startNode;
	    [SerializeField] private NodeBase targetNode;

	    [SerializeField] private Vector3Int startPosition ;
	    [SerializeField] private Vector3Int targetPosition;
#endregion

#region Dependencies
	    [SerializeField] private bool useGridNodeData;
	    
	    [SerializeField] private GridNodeDataManager gridNodeDataManager;
	    [SerializeField] private GridNodeData        gridNodeData;
	    
	    [SerializeField] private PathFinderController pathFinderController; //Path Finder Controller should be singleton?
#endregion

	    private Coroutine followPathCoroutine;

	    public void Initialized()
	    {
		    if(useGridNodeData)
				InjectGridNodeData();
		    else
			    InjectCustomGridNodeData(gridNodeData);
		    
		    InjectPathFinderController();
		    
		    //Get self position in gridNodeData
	    }

	    public void LoadGridData(GridNodeData _gridNodeData)
	    {
		    gridNodeData = _gridNodeData;
	    }
	    
	    /// <summary>
	    /// Use GridNodeData as Default Initialization.
	    /// </summary>
	    /// <param name="_gridNodeDataManager"></param>
	    private void InjectGridNodeData(GridNodeDataManager _gridNodeDataManager = null)
	    {
		    gridNodeDataManager = _gridNodeDataManager != null ? _gridNodeDataManager : FindObjectOfType<GridNodeDataManager>();

		    if (!gridNodeDataManager)
		    {
			    Debug.LogWarning($"GridNodeData Missing Reference.");
		    }
	    }
	    
	    private void InjectCustomGridNodeData(GridNodeData _gridNodeData)
	    {
		    gridNodeData = _gridNodeData;
	    }
	    
	    public void InjectPathFinderController(PathFinderController _pathFinderController = null)
	    {
		    pathFinderController = _pathFinderController != null ? _pathFinderController : FindObjectOfType<PathFinderController>();

		    if (!pathFinderController)
		    {
			    Debug.LogWarning($"{typeof(PathFinderController)} Missing Reference.");
		    }
	    }

	    public void StartFindPath()
	    {
		    if (followPathCoroutine != null)
		    {
			    StopCoroutine(followPathCoroutine);
			    followPathCoroutine = null;
		    }

		    if (!useNode)
		    {
			    startNode  = gridNodeDataManager.GetNode(startPosition );
			    targetNode = gridNodeDataManager.GetNode(targetPosition);
		    }
		    
		    var _newPathRequest = new PathRequestNodeBase(startNode, targetNode, OnPathFoundHandler);
		    
		    pathFinderController.RequestPath(_newPathRequest);
	    }
	    
	    private void OnPathFoundHandler(NodeBase[] _nodePath, bool _pathSuccessful) 
	    {
		    if (!_pathSuccessful) return;
			    
		    currentFollowedPath = _nodePath;

		    Debug.Log($"Path Found!");

		    followPathCoroutine = StartCoroutine(FollowPath());
	    }

	    private IEnumerator FollowPath()
	    { 
		    followingPath  = true;
		    
		    int _pathIndex = 0;
		    int _lastIndex = currentFollowedPath.Length - 1;

		    while (followingPath)
		    {
			    var _currentCellPos = (Vector3)currentFollowedPath[_pathIndex].NodePosition;
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
