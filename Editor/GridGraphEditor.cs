using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using MugCup_PathFinder.Runtime;
using UnityEngine;

namespace MugCup_PathFinder.Editor
{
    [CustomEditor(typeof(GridGraph))]
    public class GridGraphEditor : UnityEditor.Editor
    {
        private GridGraph gridGraph;

        private void OnEnable()
        {
            gridGraph = (GridGraph)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Vertices"))
            {
                gridGraph.GenerateValidVertices();
            }

            if (GUILayout.Button("Clear Vertex Data"))
            {
                gridGraph.ClearVertexData();
                
                var _gridNodes = FindObjectsOfType<GridNode>();
                foreach (var _node in _gridNodes)
                    _node.SetVertexNodeInit(false);
            }

            if (GUILayout.Button("Calculate Flow Field"))
            {
                gridGraph.CalculateFlowField();
            }
        }
    }
}
