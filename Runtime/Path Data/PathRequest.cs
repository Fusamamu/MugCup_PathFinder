using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
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
}
