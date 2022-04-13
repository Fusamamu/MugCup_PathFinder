using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class Agent : MonoBehaviour
    {
       
	    //Add PathFinder Automatically?

	    public IEnumerator FollowPath(PathInfo _pathInfo)
	    {
		    bool _followingPath = true;
		    
		    int _pathIndex = 0;
		    
		    transform.LookAt(_pathInfo.LookPoints[0]);

		    float _speedPercent = 1;

		    while (_followingPath) 
		    {
			    Vector2 pos2D = new Vector2 (transform.position.x, transform.position.z);
			    
			    // while (_pathInfo.turnBoundaries [pathIndex].HasCrossedLine (pos2D)) {
				   //  if (pathIndex == _pathInfo.finishLineIndex) {
					  //   followingPath = false;
					  //   break;
				   //  } else {
					  //   pathIndex++;
				   //  }
			    // }
			    //
			    // if (followingPath) {
			    //
				   //  if (pathIndex >= _pathInfo.slowDownIndex && stoppingDst > 0) {
					  //   speedPercent = Mathf.Clamp01 (_pathInfo.turnBoundaries [_pathInfo.finishLineIndex].DistanceFromPoint (pos2D) / stoppingDst);
					  //   if (speedPercent < 0.01f) {
						 //    followingPath = false;
					  //   }
				   //  }
			    //
				   //  Quaternion targetRotation = Quaternion.LookRotation (_pathInfo.lookPoints [pathIndex] - transform.position);
				   //  transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
				   //  transform.Translate (Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
			    // }

			    yield return null;
		    }
	    }
       
        

       
    }
}
