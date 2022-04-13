using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathInfo
    {
        public readonly Vector3[] LookPoints;
        //public readonly Line[] turnBoundaries;
        
        public readonly int finishLineIndex;
        public readonly int slowDownIndex;
        
        public PathInfo(Vector3[] _waypoints, Vector3 _startPos, float _turnDistance, float _stoppingDistance) 
        {
            LookPoints = _waypoints;
            
            //turnBoundaries  = new Line[lookPoints.Length];
            //finishLineIndex = turnBoundaries.Length - 1;
        
            Vector2 _previousPoint = CastVec3ToVec2(_startPos);
            
            for (var _i = 0; _i < LookPoints.Length; _i++)
            {
                Vector2 _currentPoint      = CastVec3ToVec2 (LookPoints [_i]);
                Vector2 _dirToCurrentPoint = (_currentPoint - _previousPoint).normalized;
                Vector2 _turnBoundaryPoint = _i == finishLineIndex ? _currentPoint : _currentPoint - _dirToCurrentPoint * _turnDistance;
                
                //turnBoundaries [_i] = new Line (turnBoundaryPoint, previousPoint - dirToCurrentPoint * _turnDistance);
                
                _previousPoint = _turnBoundaryPoint;
            }
        
            float dstFromEndPoint = 0;
            
            for (int i = LookPoints.Length - 1; i > 0; i--) 
            {
                dstFromEndPoint += Vector3.Distance (LookPoints [i], LookPoints [i - 1]);
                
                if (dstFromEndPoint > _stoppingDistance) {
                    slowDownIndex = i;
                    break;
                }
            }
        }
        
        private static Vector2 CastVec3ToVec2(Vector3 _pos) 
        {
            return new Vector2 (_pos.x, _pos.z);
        }
        
        public void DrawWithGizmos() {
        
            // Gizmos.color = Color.black;
            // foreach (Vector3 p in lookPoints) {
            //     Gizmos.DrawCube (p + Vector3.up, Vector3.one);
            // }
            //
            // Gizmos.color = Color.white;
            // foreach (Line l in turnBoundaries) {
            //     l.DrawWithGizmos (10);
            // }
        }
    }
}
