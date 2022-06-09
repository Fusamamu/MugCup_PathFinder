#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MugCup_PathFinder.Runtime;

namespace MugCup_PathFinder.Editor
{
    [CustomEditor(typeof(PathFinderController))]
    public class PathFinderControllerEditor : UnityEditor.Editor
    {
        private PathFinderController pathFinderController;

        private void OnEnable()
        {
            pathFinderController = (PathFinderController)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}
#endif
