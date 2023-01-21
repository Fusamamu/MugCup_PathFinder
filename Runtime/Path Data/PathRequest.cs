using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	[Serializable]
	public class PathRequestVec3 : PathRequest<Vector3>
	{
		public PathRequestVec3(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) : base(_start, _end, _callback)
		{
		}
	}

	[Serializable]
	public class PathRequestNodeBase : PathRequest<GridNode>
	{
		public PathRequestNodeBase(GridNode _start, GridNode _end, Action<GridNode[], bool> _callback) : base(_start, _end, _callback)
		{
		}
	}

	[Serializable]
	public class PathRequest<T>
	{
	    public readonly T PathStart;
	    public readonly T PathEnd  ;
	    
	    public readonly Action<T[], bool> Callback;
	
	    public PathRequest(T _start, T _end, Action<T[], bool> _callback) 
	    {
	        PathStart = _start   ;
	        PathEnd   = _end     ;
	        Callback  = _callback;
	    }
	}
	
	// public class PathFindingRequest
	// {
	// 	public Vector3 src;
	// 	public Vector3 dst;
	//
	// 	internal bool done;
	// 	internal Path result;
	//
	// 	public bool IsDone {
	// 		get => done;
	// 	}
	//
	// 	public PathFindingRequest (Vector3 start, Vector3 end) {
	// 		this.src = start;
	// 		this.dst = end;
	// 	}
	//
	// 	public void Queue () {
	// 		PathFindiningSystem.instance.QueueJob(this);
	// 	}
	//
	// 	public Path GetResult () {
	// 		if (!done) {
	// 			Debug.LogError("Path is not done yet. Please wait for the IsDone function to return true.");
	// 		}
	// 		return result;
	// 	}
	//
	// }
}
