#if UNITY_EDITOR
using MugCup_PathFinder.Runtime;
using UnityEditor;
using UnityEngine;

namespace MugCup_PathFinder.Editor
{
    [CustomEditor(typeof(GridNodeDataManager))]
    public class GridNodeDataEditor : UnityEditor.Editor
    {
        private GridNodeDataManager gridNodeDataManager;

        private SerializedProperty gridDataSetting;
        private SerializedProperty gridNodes;

        private void OnEnable()
        {
            gridNodeDataManager = (GridNodeDataManager)target;

            gridDataSetting = serializedObject.FindProperty("gridNodeDataSetting");
            gridNodes       = serializedObject.FindProperty("gridNodes"          );
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Initialize"))
            {
                gridNodeDataManager.InitializeGridNode();
            }
            
            if (GUILayout.Button("Clear Data"))
            {
                gridNodeDataManager.ClearData();
            }
        }
    }
}
#endif
