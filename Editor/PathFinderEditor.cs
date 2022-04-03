#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using MugCup_PathFinder.Runtime;
using UnityEditor;
using UnityEngine;

namespace MugCup_PathFinder.Editor
{
    [CustomEditor(typeof(PathFinder))]
    public class PathFinderEditor : UnityEditor.Editor
    {
        private PathFinder pathFinder;
        private Transform pathFinderHandleTransform;
        
        private Texture2D topLogo;

        private SerializedProperty gridSize;
        private SerializedProperty startPos;
        private SerializedProperty targetPos;

        private SerializedProperty gridNodes;
        private SerializedProperty pathNodes;

        private void OnEnable()
        {
            pathFinder = (PathFinder)target;
            pathFinderHandleTransform = pathFinder.transform;
            
            topLogo = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.mugcupp.mugcup-pathfinder/PackageResources/IconImages/PathFinderIcon_S.png");

            gridSize  = serializedObject.FindProperty("gridSize");
            startPos  = serializedObject.FindProperty("startPosition");
            targetPos = serializedObject.FindProperty("targetPosition");
            gridNodes = serializedObject.FindProperty("gridNodes");
            pathNodes = serializedObject.FindProperty("pathNodes");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Label(topLogo);
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(gridSize );
            EditorGUILayout.PropertyField(startPos );
            EditorGUILayout.PropertyField(targetPos);
            EditorGUILayout.PropertyField(gridNodes);
            EditorGUILayout.PropertyField(pathNodes);
            serializedObject.ApplyModifiedProperties();


            EditorGUILayout.BeginHorizontal("Box");

            if (GUILayout.Button("Generate Path"))
            {
                var _pathNodes = pathFinder.GeneratePath();
            }

            if (GUILayout.Button("Clear Path"))
            {
                pathFinder.Clear();
            }
            EditorGUILayout.EndHorizontal();

        }

        private void OnSceneGUI()
        {
            var _gridSize = pathFinder.GridSize;
            
            
            DrawGridGizmos(_gridSize);
            DrawCell( startPos.vector3IntValue, Color.green  );
            DrawCell(targetPos.vector3IntValue, Color.magenta);
        }

        private void DrawCell(Vector3 _pos, Color _color)
        {
            Vector3[] _cellVerts = 
            {
                new Vector3(_pos.x,     0, _pos.z    ),
                new Vector3(_pos.x + 1, 0, _pos.z    ),
                new Vector3(_pos.x + 1, 0, _pos.z + 1),
                new Vector3(_pos.x    , 0, _pos.z + 1)
            };

            Handles.DrawSolidRectangleWithOutline(_cellVerts, _color, Color.blue );
        }
        
        private void DrawGridGizmos(Vector3Int _gridSize)
        {
            for (int _level = 0; _level < _gridSize.y; _level++)
            { 
                DrawGridFloor(_gridSize, _level);
            }
        }
        
        private void DrawGridFloor(Vector3Int _gridSize, int _level = 0)
        {
            Handles.color = new Color(0.2f * _level, 1, 1);

            Vector3 _p0 = Vector3.zero;
            Vector3 _p1 = Vector3.zero;

            for (var _i = 0; _i <= _gridSize.z; _i++)
            {
                for (var _j = 0; _j < _gridSize.x; _j++)
                {
                    _p0 = new Vector3(_j,     _level, _i);
                    _p1 = new Vector3(_j + 1, _level, _i);

                    var _worldP0 = pathFinderHandleTransform.TransformPoint(_p0);
                    var _worldP1 = pathFinderHandleTransform.TransformPoint(_p1);

                    Handles.DrawLine(_worldP0, _worldP1);
                }
            }

            for (var _j = 0; _j <= _gridSize.x; _j++)
            {
                for (var _i = 0; _i < _gridSize.z; _i++)
                {
                    _p0 = new Vector3(_j, _level, _i)   ;
                    _p1 = new Vector3(_j, _level, _i + 1);

                    var _worldP0 = pathFinderHandleTransform.TransformPoint(_p0);
                    var _worldP1 = pathFinderHandleTransform.TransformPoint(_p1);

                    Handles.DrawLine(_worldP0, _worldP1);
                }
            }
        }
    }
}
#endif
