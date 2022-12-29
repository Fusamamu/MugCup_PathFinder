#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using MugCup_PathFinder.Runtime;
using UnityEditor;
using UnityEngine;

namespace MugCup_PathFinder.Editor
{
    [CustomEditor(typeof(FlowField))]
    public class FlowFieldEditor : UnityEditor.Editor
    {
        private FlowField flowField;

        private void OnEnable()
        {
            flowField = (FlowField)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Grid Nodes"))
            {
                flowField.GenerateGrid();
            }

            if (GUILayout.Button("Clear Grid"))
            {
                flowField.ClearGrid();
            }

            if (GUILayout.Button("Start Search"))
            {
                flowField.StartSearch();
            }

            if (GUILayout.Button("Reset"))
            {
                flowField.Reset();
            }

            if (GUILayout.Button("Populate Agents"))
            {
                flowField.PopulateAgents();
            }

            if (GUILayout.Button("Update Node World Position"))
            {
                flowField.UpdateNodeWorldPosition();
            }

            if (GUILayout.Button("Update Structure Node"))
            {
                flowField.UpdateStructureNodes();
            }
        }
    }
}
#endif
