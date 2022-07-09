// #if UNITY_EDITOR
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using MugCup_PathFinder.Runtime;
//
// namespace MugCup_PathFinder.Editor
// {
//     [CustomEditor(typeof(PathFinderController))]
//     public class PathFinderControllerEditor : UnityEditor.Editor
//     {
//         
//         private enum GridDataSettingMode
//         {
//             UseGridDataSetting, Custom
//         }
//
//         private GridDataSettingMode dataSettingMode = GridDataSettingMode.Custom;
//         
//         
//         private PathFinderController pathFinderController;
//         
//
//
//         private SerializedProperty gridDataSetting;
//         private SerializedProperty gridSize;
//
//         private void OnEnable()
//         {
//             pathFinderController = (PathFinderController)target;
//
//             gridDataSetting = serializedObject.FindProperty("gridNodeDataSetting");
//             gridSize        = serializedObject.FindProperty("gridSize");
//         }
//
//         public override void OnInspectorGUI()
//         {
//
//             DrawDefaultInspector();
//
//             if (GUILayout.Button("Request Path"))
//             {
//                 pathFinderController.RequestPath();
//             }
//
//             // serializedObject.Update();
//             //
//             // dataSettingMode = (GridDataSettingMode)EditorGUILayout.EnumPopup("Grid Data Setting Mode", dataSettingMode);
//             //
//             // switch (dataSettingMode)
//             // {
//             //     case GridDataSettingMode.UseGridDataSetting:
//             //         EditorGUILayout.PropertyField(gridDataSetting);
//             //         break;
//             //     case GridDataSettingMode.Custom:
//             //         EditorGUILayout.PropertyField(gridSize);
//             //         break;
//             // }
//             //
//             // serializedObject.ApplyModifiedProperties();
//         }
//     }
// }
// #endif
