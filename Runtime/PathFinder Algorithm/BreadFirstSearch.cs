using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public static class BreadFirstSearch<T> where T : class, INode
    {
        public static IEnumerator BreadthFirstSearch(Vector3Int _srcPos, GridData<T> _gridNodeData)
        {
            var _openSet  = new Queue<T>();
            var _closeSet = new HashSet<T>();
            
            var _srcNode = GridUtility.GetNode<T>(_srcPos, _gridNodeData);
            _srcNode.NodeParent = null;
            
            _openSet.Enqueue(_srcNode);
            _closeSet.Add(_srcNode);

            while (_openSet.Count > 0)
            {
                var _currentNode = _openSet.Dequeue();

                var _neighbors = _currentNode.GetNeighbors<T>();

                foreach (var _node in _neighbors)
                {
                    _node.NodeParent ??= _currentNode;

                    if (!_closeSet.Contains(_node))
                    {
                        _openSet.Enqueue(_node);
                        _closeSet.Add(_node);

                        yield return null;
                    }
                }
            }
            
            Debug.Log("Flow Field Completed !");
        }
    }
}
