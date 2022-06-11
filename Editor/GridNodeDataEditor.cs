#if UNITY_EDITOR
using System;
using MugCup_PathFinder.Runtime;
using UnityEditor;
using UnityEngine;

namespace MugCup_PathFinder.Editor
{
    [CustomEditor(typeof(GridNodeData))]
    public class GridNodeDataEditor : UnityEditor.Editor
    {
        private GridNodeData gridNodeData;

        private SerializedProperty gridDataSetting;
        private SerializedProperty gridNodes;

        private void OnEnable()
        {
            gridNodeData = (GridNodeData)target;

            gridDataSetting = serializedObject.FindProperty("gridNodeDataSetting");
            gridNodes       = serializedObject.FindProperty("gridNodes");

        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(gridDataSetting);
            EditorGUILayout.PropertyField(gridNodes);

            if (GUILayout.Button("Initialize"))
            {
                gridNodeData.InitializeGridNode();
            }

            if (GUILayout.Button("Clear Data"))
            {
                gridNodeData.ClearData();
            }

        }
    }
}
#endif
