using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Graphs;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public static class Utilities
    {
        public static T[] CovertAllNodes<T>(GridNode[] _nodeBases) where T : GridNode
        {
            var _newArray = new T[_nodeBases.Length];

            for (var _i = 0; _i < _nodeBases.Length; _i++)
            {
                var _newType = _nodeBases[_i] as T;
                
                if(_newType == null)
                    continue;

                _newArray[_i] = _newType;
            }

            return _newArray;
        }
        
        public static Vector3Int CastVec3ToVec3Int(Vector3 _pos)
        {
            var _xPos = (int)Mathf.Round(_pos.x);
            var _yPos = (int)Mathf.Round(_pos.y);
            var _zPos = (int)Mathf.Round(_pos.z);

            return new Vector3Int(_xPos, _yPos, _zPos);
        }

        public static Vector3Int AsVector3Int(this int3 _int3)
        {
            return new Vector3Int(_int3.x, _int3.y, _int3.z);
        }

        public static int3 AsInt3(this Vector3Int _vector3)
        {
            return new int3(_vector3.x, _vector3.y, _vector3.z);
        }
        
        public static int3 Vec3IntToInt3(Vector3Int _vector3)
        {
            return new int3(_vector3.x, _vector3.y, _vector3.z);
        }
        
        // private Vector3Int CastVec3ToVec3Int(Vector3 _value)
        // {
        //     return new Vector3Int((int)_value.x, (int)_value.y, (int)_value.z);
        // }
    }
}
