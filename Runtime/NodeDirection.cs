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

        public static DirectionChange GetDirectionChangeTo (this NodeDirection current, NodeDirection next) 
        {
            if (current == next) {
                return DirectionChange.None;
            }
            else if (current + 1 == next || current - 3 == next) {
                return DirectionChange.TurnRight;
            }
            else if (current - 1 == next || current + 3 == next) {
                return DirectionChange.TurnLeft;
            }
            return DirectionChange.TurnAround;
        }

        public static Vector3 GetHalfVector (this NodeDirection direction) {
            return halfVectors[(int)direction];
        }

        public static Quaternion GetRotation (this NodeDirection direction) {
            return rotations[(int)direction];
        }
    }
}
