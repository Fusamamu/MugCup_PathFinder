using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public struct PathRequest<T>
	{
	    public T pathStart;
	    public T pathEnd  ;
	    
	    public Action<T[], bool> callback;
	
	    public PathRequest(T _start, T _end, Action<T[], bool> _callback) 
	    {
	        pathStart = _start   ;
	        pathEnd   = _end     ;
	        callback  = _callback;
	    }
	}
}
