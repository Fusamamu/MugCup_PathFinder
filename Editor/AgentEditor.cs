using System;
using UnityEditor;
using UnityEngine;
using MugCup_PathFinder.Runtime;

namespace MugCup_PathFinder.Editor
{
    [CustomEditor(typeof(Agent))]
    public class AgentEditor : UnityEditor.Editor
    {
        private Agent agent;

        private void OnEnable()
        {
            agent = (Agent)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Demo Use GridNodeDataManager"))
            {
                agent.SetUseGridNodeDataManager(true);
                agent.Initialized();
            }
            
            if (GUILayout.Button("Start Find Path"))
            {
                agent.RequestPath();
            }
        }
    }
}
