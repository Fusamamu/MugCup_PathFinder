using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    /// <summary>
    /// Containing methods and algorithm for selecting nodes in grid
    /// with different patterns. 
    /// </summary>
    public static class GridAreaSelection
    {
        /// <summary>
        /// Get nodes' position in cross pattern expanding in x and z axis.
        /// </summary>
        /// <param name="_nodePos">Selected Node Position</param>
        /// <param name="_range">Defined range in both x and y axis</param>
        public static IEnumerable<Vector3Int> GetCrossNodesXZ(Vector3Int _nodePos, int _range = 1)
        {
            if (_range <= 0) return null;
            
            var _crossPositions = new List<Vector3Int>();

            for (var _i = 0; _i < _range; _i++)
            {
                var _forwardPos  = _nodePos + Vector3Int.forward * _i;
                var _rightPos    = _nodePos + Vector3Int.right   * _i;
                var _backwardPos = _nodePos + Vector3Int.back    * _i;
                var _leftPos     = _nodePos + Vector3Int.left    * _i;
                
                _crossPositions.AddRange(new List<Vector3Int> {
                    _forwardPos,
                    _rightPos,
                    _backwardPos,
                    _leftPos
                });
            }

            return _crossPositions;
        }

        /// <summary>
        /// Get nodes' position in diagonal pattern expanding in x and z axis.
        /// </summary>
        /// <param name="_nodePos">Selected Node Position</param>
        /// <param name="_range">Defined range in both x and y axis</param>
        /// <returns></returns>
        public static IEnumerable<Vector3Int> GetDiagonalNodesXZ(Vector3Int _nodePos, int _range = 1)
        {
            if (_range <= 0) return null;
            
            var _diagonalPositions = new List<Vector3Int>();

            for (var _i = 1; _i < _range; _i++)
            {
                var _forwardLeftPos   = _nodePos + (Vector3Int.forward + Vector3Int.left ) * _i;
                var _forwardRightPos  = _nodePos + (Vector3Int.forward + Vector3Int.right) * _i;
                var _backwardLeftPos  = _nodePos + (Vector3Int.back    + Vector3Int.left ) * _i;
                var _backwardRightPos = _nodePos + (Vector3Int.back    + Vector3Int.right) * _i;
                
                _diagonalPositions.AddRange(new List<Vector3Int> {
                    _forwardLeftPos,
                    _forwardRightPos,
                    _backwardLeftPos,
                    _backwardRightPos
                });
            }

            return _diagonalPositions;
        }

        /// <summary>
        /// Get nodes' position in cross pattern expanding in x and z axis.
        /// But return only the most outer nodes' position.
        /// </summary>
        /// <param name="_nodePos">Selected Node Position</param>
        /// <param name="_range">Defined range in both x and y axis</param>
        /// <returns></returns>
        public static IEnumerable<Vector3Int> GetOuterCrossNodesXZ(Vector3Int _nodePos, int _range = 1)
        {
            if (_range <= 0) return null;
            
            var _outerCrossPositions = new List<Vector3Int>();

            var _forwardPos  = _nodePos + Vector3Int.forward * _range;
            var _rightPos    = _nodePos + Vector3Int.right   * _range;
            var _backwardPos = _nodePos + Vector3Int.back    * _range;
            var _leftPos     = _nodePos + Vector3Int.left    * _range;
                
            _outerCrossPositions.AddRange(new List<Vector3Int> {
                _forwardPos,
                _rightPos,
                _backwardPos,
                _leftPos
            });

            return _outerCrossPositions;
        }

        public static IEnumerable<Vector3Int> GetOuterFirstQuadrantXZ(Vector3Int _nodePos, int _range = 1)
        {
            if (_range <= 0) return null;

            var _outerFirstQuadrantPosition = new List<Vector3Int>();
            
            var _startPos = _nodePos + Vector3Int.forward * _range;

            /*Number of all outer nodes' position.*/
            var _outerPosCount = _range - 1;
            
            for (var _i = 0; _i < _outerPosCount; _i++)
            {
                if(_i == 0) continue;
                
                _startPos +=  Vector3Int.back + Vector3Int.right;

                var _newPosition = new Vector3Int(_startPos.x, _startPos.y, _startPos.z);
                
                _outerFirstQuadrantPosition.Add(_newPosition);
            }
            
            return _outerFirstQuadrantPosition;
        }
        
        public static IEnumerable<Vector3Int> GetCircleAreaDistanceBased(Vector3Int _gridSize, Vector3Int _center, float _radius)
        {
            var _circleAreaPoints = new List<Vector3Int>();
            
            for (var _x = 0; _x < _gridSize.x; _x++)
                for (var _z = 0; _z < _gridSize.z; _z++)
                {
                    var _checkPos = new Vector3Int(_x, 0, _z);
                        
                    if(IsInsideCircle(_checkPos, _center, _radius))
                        _circleAreaPoints.Add(_checkPos);
                }

            return _circleAreaPoints;
        }

        public static IEnumerable<Vector3Int> GetRectAreaDistanceBased(Vector3Int _gridSize, Vector3Int _center, float _radius)
        {
            for (var _x = 0; _x < _gridSize.x; _x++)
            for (var _z = 0; _z < _gridSize.z; _z++)
            {
                var _checkPos = new Vector3Int(_x, 0, _z);

                if (IsInsideRect(_checkPos, _center, _radius))
                    yield return _checkPos;
            }
        }

        public static bool IsInsideCircle(Vector3Int _checkedPos, Vector3Int _center, float _radius)
        {
            float _dx = _center.x - _checkedPos.x;
            float _dz = _center.z - _checkedPos.z;

            float _distance = Mathf.Sqrt(_dx*_dx + _dz*_dz);
            
            return _distance <= _radius;
        }

        public static bool IsInsideRect(Vector3Int _checkedPos, Vector3Int _center, float _radius)
        {
            float _distX = Mathf.Abs(_center.x - _checkedPos.x);
            float _distY = Mathf.Abs(_center.z - _checkedPos.z);

            return _distX <= _radius && _distY <= _radius;
        }
    }
}
