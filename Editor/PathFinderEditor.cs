#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using MugCup_PathFinder.Runtime;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Component = UnityEngine.Component;

namespace MugCup_PathFinder.Editor
{
    [CustomEditor(typeof(PathFinder))]
    public class PathFinderEditor : UnityEditor.Editor
    {
        private PathFinder pathFinder;
        private Transform pathFinderHandleTransform;
        
        private Texture2D topLogo;

        private SerializedProperty startPathType;
        private SerializedProperty endPathType;

        private SerializedProperty agent;

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

            startPathType   = serializedObject.FindProperty("StartPathType");
            endPathType     = serializedObject.FindProperty("EndPathType");
            
            agent        = serializedObject.FindProperty("agent"         );
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
            
            //GUI.color = new Color(120 / 255f, 120 / 255f, 120 / 255f);
            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.LabelField("Start Path Setting", EditorStyles.whiteBoldLabel);
            EditorGUILayout.PropertyField(startPathType);
            
            if (pathFinder.StartPathType == PathFinder.TargetType.UseAgent)
            {
                EditorGUILayout.HelpBox(
                    "Drop an Agent you wish to use the path finder to move object along the path. " +
                    "Otherwise, if set none, the script will try to get the agent from attached object"
                    , MessageType.None);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(agent, new GUIContent("Agent", "An object attached with Agent component."));
                //EditorGUILayout.
                if (GUILayout.Button("GET AGENT", EditorStyles.miniButton))
                {
                    GetComponent(agent);
                }
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.LabelField("End Path Setting", EditorStyles.whiteBoldLabel);
            EditorGUILayout.PropertyField(endPathType);
            
            if (pathFinder.EndPathType == PathFinder.TargetType.UseAgent)
            {
                EditorGUILayout.HelpBox(
                    "Drop an Agent you wish to use the path finder to move object along the path. " +
                    "Otherwise, if set none, the script will try to get the agent from attached object"
                    , MessageType.None);

                EditorGUILayout.PropertyField(agent, new GUIContent("Agent", "An object attached with Agent component."));
            }
            
            EditorGUILayout.EndVertical();


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
            
            EditorGUILayout.LabelField("Mug Cup Studio.", EditorStyles.centeredGreyMiniLabel);
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
            
            DrawLabelPanel();
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

        private void DrawPath(GridNode[] _path)
        {
            Handles.color = Color.red;

            for (var _i = 0; _i < _path.Length - 1; _i++)
            {
                var _p0 = _path[_i].NodeGridPosition;
                var _p1 = _path[_i + 1].NodeGridPosition;
      
                var _thickness = 7;
                
                Handles.DrawBezier(_p0, _p1, _p0, _p1, Color.red, null, _thickness);
            }
        }

        private void DrawArrow(GridNode[] _path)
        {
            Handles.color = Color.red;
            
            //Handles.ArrowCap( 0, t.transform.position, t.transform.rotation * Quaternion.Euler( 0, 90, 0 ), arrowSize );
            Handles.ArrowHandleCap(0, pathFinderHandleTransform.position + new Vector3(5,5,5), Quaternion.identity, 20, EventType.Ignore);
        }

        private void DisplayText(GridNode[] _nodes)
        {
            foreach (var _node in _nodes)
            {
                var _textPos = _node.NodeGridPosition + Vector3Int.up;
                var _nodeCost = 
                    $"F : {_node.F_Cost}\n" +
                    $"G : {_node.G_Cost}\n" +
                    $"H : {_node.H_Cost}";
                
                Handles.Label(_textPos, _nodeCost);
            }
        }

        private static GUIStyle miniPanel;
        
        private void DrawLabelPanel()
        {
            if (miniPanel == null)
            {
                miniPanel = new GUIStyle(GUI.skin.window)
                {
                    fixedWidth  = 60,
                    fixedHeight = 60
                };

                miniPanel.padding.bottom -= 27;
                miniPanel.padding.left   -= 7;
                miniPanel.padding.right  -= 7;
                
                miniPanel.margin = new RectOffset();
                
                miniPanel.richText = true;
            }
            
            Handles.Label(Vector3.zero, "sajfsd", miniPanel);
            
            // if (!EditorPrefs.GetBool(MENU_PATH, false))
            //     return;
            // if (meshFilter.sharedMesh == null)
            //     return;
            //
            // var _style = new GUIStyle();
            //
            // if (_style == null)
            // {
            //     _style = new GUIStyle(GUI.skin.window);
            //     _style.padding.bottom -= 27;
            //     _style.padding.left -= 7;
            //     _style.padding.right -= 7;
            //     _style.margin = new RectOffset();
            //     _style.richText = true;
            // }
            // // 同じ座標が存在するのでキャッシュ
            // var dic = new Dictionary<Vector3, List<int>>();
            // var mesh = meshFilter.sharedMesh;
            // for (var index = 0; index < mesh.uv.Length; index++)
            // {
            //     var pos = meshFilter.transform.position + meshFilter.transform.rotation * mesh.vertices[index];
            //     if (!dic.ContainsKey(pos))
            //         dic.Add(pos, new List<int>());
            //     dic[pos].Add(index);
            // }
            // foreach (var pair in dic)
            // {
            //     _builder.Clear();
            //     foreach (var index in pair.Value)
            //         if (mesh.colors.Length > index)
            //         {
            //             var color = ColorUtility.ToHtmlStringRGB(mesh.colors[index]);
            //             _builder.AppendLine($"<color=#{color}>uv:{mesh.uv[index]}</color>");
            //         }
            //         else
            //         {
            //             _builder.AppendLine("uv:" + mesh.uv[index]);
            //         }
            //     Handles.Label(pair.Key, _builder.ToString(), _style);
            // }
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

        private void GetComponent(SerializedProperty _property)
        {
            var _object = _property.serializedObject.targetObject as Component;

            if (_object == null)
            {
                Debug.Log("GameObject Not Found");
                return;
            }

            var _type = _property.serializedObject.targetObject.GetType();

            var _fieldInfo = _type.GetField(_property.propertyPath, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            
            if (_fieldInfo == null)
            {
                return;
            }

            var _filedType = _fieldInfo.FieldType;

            var _components = _object.GetComponentsInChildren(_filedType);

            if (_components.Length == 0)
            {
                Debug.Log("Component Not Found");
            }else if (_components.Length == 1)
            {
                _property.objectReferenceValue = _components[0];
            }
            else
            { 
                PopupWindow.Show(Rect.zero , new Popup(_property, _components));
            }
        }
        
        private class Popup : PopupWindowContent
        {
            private readonly TreeView treeView;
            
            public Popup(SerializedProperty _property, Component[] _components)
            {
                treeView = new PopupTreeView(new TreeViewState(), this, _property, _components);
                GUIUtility.keyboardControl = treeView.treeViewControlID;
            }
            
            public override void OnGUI(Rect _rect)
            {
                treeView.OnGUI(_rect);
            }
            
            private class PopupTreeView : TreeView
            {
                private readonly SerializedProperty property;
                private readonly Popup              popup;
                private readonly Component[]        components;
     
                public PopupTreeView(TreeViewState _state, Popup _popup, SerializedProperty _property, Component[] _components) : base(_state)
                {
                    property   = _property;
                    popup      = _popup;
                    components = _components;
                    
                    Reload();
                }
     
                public PopupTreeView(TreeViewState _state, MultiColumnHeader _multiColumnHeader) : base(_state, _multiColumnHeader)
                {
                }
                 
                protected override TreeViewItem BuildRoot()
                {
                    var _root = new TreeViewItem(-1, -1, "root");
                    
                    for (var _i = 0; _i < components.Length; ++_i)
                    {
                        _root.AddChild(new TreeViewItem(_i, 0, components[_i].name));
                    }
                    
                    return _root;
                }
     
                protected override void SingleClickedItem(int _id)
                {
                    DecideObject();
                }
                
                private void DecideObject()
                {
                    if (state.selectedIDs.Count <= 0)
                        return;
                    
                    property.serializedObject.Update();
                    property.objectReferenceValue = components[state.selectedIDs.First()];
                    property.serializedObject.ApplyModifiedProperties();
                    
                    popup.editorWindow.Close();
                }
            }
        }
    }
}
#endif
