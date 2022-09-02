using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public enum NodeDirection
    {
        None, North, East, South, West
    }
    
    public enum DirectionChange 
    {
        None, TurnRight, TurnLeft, TurnAround
    }
    
    public static class DirectionExtensions {

        static Vector3[] halfVectors = 
        {
            Vector3.forward * 0.5f,
            Vector3.right * 0.5f,
            Vector3.back * 0.5f,
            Vector3.left * 0.5f
        };

        static Quaternion[] rotations = 
        {
            Quaternion.identity,
            Quaternion.Euler(0f, 90f, 0f),
            Quaternion.Euler(0f, 180f, 0f),
            Quaternion.Euler(0f, 270f, 0f)
        };

        public static float GetAngle (this NodeDirection direction) {
            return (float)direction * 90f;
        }

        public static DirectionChange GetDirectionChangeTo (this NodeDirection _current, NodeDirection _next) 
        {
            if (_current == _next) {
                return DirectionChange.None;
            }
            else if (_current + 1 == _next || _current - 3 == _next) {
                return DirectionChange.TurnRight;
            }
            else if (_current - 1 == _next || _current + 3 == _next) {
                return DirectionChange.TurnLeft;
            }
            
            return DirectionChange.TurnAround;
        }

        public static Vector3 GetHalfVector (this NodeDirection _direction) {
            return halfVectors[(int)_direction];
        }

        public static Quaternion GetRotation (this NodeDirection _direction) {
            return rotations[(int)_direction];
        }
    }
}
