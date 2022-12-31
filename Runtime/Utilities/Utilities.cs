using System.Collections;
using System.Collections.Generic;
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
    }
}
