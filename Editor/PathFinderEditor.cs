#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MugCup_PathFinder.Runtime;
using UnityEditor;
using UnityEditor.AnimatedValues;
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

        private SerializedProperty maxIteration;

        private bool toggleInfoDisplaySetting;
        private bool togglePath;
        private bool togglePathNodeCost;
        private bool toggleAllNodeCost;
        
        
        private AnimBool animToggle = new AnimBool();

        private void OnEnable()
        {
            pathFinder = (PathFinder)target;
            pathFinderHandleTransform = pathFinder.transform;
            
            topLogo = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.mugcupp.mugcup-pathfinder/PackageResources/IconImages/PathFinderIcon_Header.png");

            gridSize     = serializedObject.FindProperty("gridSize"      );
            startPos     = serializedObject.FindProperty("startPosition" );
            targetPos    = serializedObject.FindProperty("targetPosition");
            gridNodes    = serializedObject.FindProperty("gridNodes"     );
            pathNodes    = serializedObject.FindProperty("pathNodes"     );
            maxIteration = serializedObject.FindProperty("maxIteration"  );
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");

            GUILayout.Label(topLogo);
            
            EditorGUILayout.EndHorizontal();

            GUI.color = new Color(120 / 255f, 120 / 255f, 120 / 255f);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.PropertyField(gridSize );
            EditorGUILayout.PropertyField(startPos );
            EditorGUILayout.PropertyField(targetPos);
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            
            GUI.color = Color.white;
            EditorGUILayout.PropertyField(gridNodes);
            EditorGUILayout.PropertyField(pathNodes);
            
            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.LabelField("Path Finder Setting");

            EditorGUILayout.PropertyField(maxIteration);
            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal("Box");

            if (GUILayout.Button("Generate Path"))
            {
                Undo.RecordObject(pathFinder, "Path Generated");
                EditorUtility.SetDirty(pathFinder);
                
                var _pathNodes = pathFinder.GeneratePath();

                togglePath = true;
                EditorWindow _view = EditorWindow.GetWindow<SceneView>();
                _view.Repaint();
            }

            if (GUILayout.Button("Clear Path"))
            {
                pathFinder.ClearPath();
            }
            EditorGUILayout.EndHorizontal();
            
            GUILayout.BeginVertical("Box");
            
            toggleInfoDisplaySetting = GUILayout.Toggle(toggleInfoDisplaySetting, "Information Display Setting", GUI.skin.GetStyle("foldout"), GUILayout.ExpandWidth(true), GUILayout.Height(18));
            animToggle.target = toggleInfoDisplaySetting;

            EditorGUI.indentLevel++;
            if (EditorGUILayout.BeginFadeGroup(animToggle.faded))
            {
                togglePath         = EditorGUILayout.ToggleLeft(new GUIContent("Display path", "Display generated path if having one"), togglePath        );
                togglePathNodeCost = EditorGUILayout.ToggleLeft("Display path's nodes cost", togglePathNodeCost);
                toggleAllNodeCost  = EditorGUILayout.ToggleLeft("Display all node's cost",   toggleAllNodeCost );
                
                EditorWindow _view = EditorWindow.GetWindow<SceneView>();
                _view.Repaint();
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUI.indentLevel--;
            
            GUILayout.EndVertical();
            
            EditorGUILayout.LabelField("Mug Cup Studio.");
        }

        private void OnSceneGUI()
        {
            var _gridSize = pathFinder.GridSize;
            
            DrawGridGizmos(_gridSize);
            
            DrawCell( startPos.vector3IntValue, Color.green  );
            DrawCell(targetPos.vector3IntValue, Color.magenta);

            if (pathFinder.HasPath)
            {
                if(togglePath)
                    DrawPath(pathFinder.PathNodes);
                if(togglePathNodeCost)
                    DisplayText(pathFinder.PathNodes);
                if (toggleAllNodeCost)
                    DisplayText(pathFinder.GridNodes);
            }
        }

        private void DrawCell(Vector3 _pos, Color _color)
        {
            var _offSet = new Vector3(-0.5f, 0, -0.5f);
            
            Vector3[] _cellVerts = 
            {
                new Vector3(_pos.x,     0, _pos.z    ) + _offSet,
                new Vector3(_pos.x + 1, 0, _pos.z    ) + _offSet,
                new Vector3(_pos.x + 1, 0, _pos.z + 1) + _offSet,
                new Vector3(_pos.x    , 0, _pos.z + 1) + _offSet
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

                    var _offSet = new Vector3(-0.5f, 0, -0.5f);

                    _worldP0 += _offSet;
                    _worldP1 += _offSet;

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
                    
                    var _offSet = new Vector3(-0.5f, 0, -0.5f);

                    _worldP0 += _offSet;
                    _worldP1 += _offSet;

                    Handles.DrawLine(_worldP0, _worldP1);
                }
            }
        }

        private void DrawPath(NodeBase[] _path)
        {
            Handles.color = Color.red;

            for (var _i = 0; _i < _path.Length - 1; _i++)
            {
                var _p0 = _path[_i].NodePosition;
                var _p1 = _path[_i + 1].NodePosition;
      
                var _thickness = 7;
                
                Handles.DrawBezier(_p0, _p1, _p0, _p1, Color.red, null, _thickness);
            }
        }

        private void DrawArrow(NodeBase[] _path)
        {
            Handles.color = Color.red;
            
            //Handles.ArrowCap( 0, t.transform.position, t.transform.rotation * Quaternion.Euler( 0, 90, 0 ), arrowSize );
            Handles.ArrowHandleCap(0, pathFinderHandleTransform.position + new Vector3(5,5,5), Quaternion.identity, 20, EventType.Ignore);
        }

        private void DisplayText(NodeBase[] _nodes)
        {
            foreach (var _node in _nodes)
            {
                var _textPos = _node.NodePosition + Vector3Int.up;
                var _nodeCost = 
                    $"F : {_node.F_Cost}\n" +
                    $"G : {_node.G_Cost}\n" +
                    $"H : {_node.H_Cost}";
                
                Handles.Label(_textPos, _nodeCost);
            }
        }
        
        public static void HorizontalLine(float _height = 1, float _width = -1, Vector2 _margin = new Vector2())
        {
            GUILayout.Space(_margin.x);

            var _rect = EditorGUILayout.GetControlRect(false, _height);
            if (_width > -1)
            {
                var _centerX = _rect.width / 2;
                _rect.width = _width;
                _rect.x += _centerX - _width / 2;
            }

            Color _color = EditorStyles.label.active.textColor;
            _color.a = 0.5f;
            EditorGUI.DrawRect(_rect, _color);

            GUILayout.Space(_margin.y);
        }

    }
}
#endif
