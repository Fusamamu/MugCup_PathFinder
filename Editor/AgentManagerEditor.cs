#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MugCup_PathFinder.Runtime;

namespace MugCup_PathFinder.Editor
{
    [CustomEditor(typeof(AgentManager))]
    public class AgentManagerEditor : UnityEditor.Editor
    {
        private AgentManager agentManager;

        private void OnEnable()
        {
            agentManager = (AgentManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Populate Agents"))
            {
                agentManager.PopulateAgents();
            }

            if (GUILayout.Button("Clear Agents"))
            {
                agentManager.ClearAgents();
            }
        }
    }
}
#endif
