#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using MugCup_PathFinder.Runtime;

namespace MugCup_PathFinder.Editor
{
    [CustomEditor(typeof(PathAgent))]
    public class AgentEditor : UnityEditor.Editor
    {
        private PathAgent pathAgent;

        private void OnEnable()
        {
            pathAgent = (PathAgent)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // if (GUILayout.Button("Demo Use GridNodeDataManager"))
            // {
            //     agent.SetUseGridNodeDataManager(true);
            //     agent.Initialized();
            // }
            
            if (GUILayout.Button("Start Find Path"))
            {
                pathAgent
                    .StopFollowPath()
                    .RequestPath();
            }
        }
    }
}
#endif
