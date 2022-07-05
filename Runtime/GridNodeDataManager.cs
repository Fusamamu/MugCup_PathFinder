using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    //Need to Change to global manager for grid node
    public class GridNodeDataManager : MonoBehaviour
    {
        /// <summary>
        /// Use GridNodeDataSetting as first priority.
        /// </summary>
        [SerializeField] private GridNodeDataSetting gridNodeDataSetting;
        
        /// <summary>
        /// Data must only initialize from GridNodeDataSetting 
        /// </summary>
        [ReadOnly] [SerializeField] private Vector3Int gridSize ;
        [ReadOnly] [SerializeField] private NodeBase[] gridNodes;

        [SerializeField] private bool isInit;

        private void Awake()
        {
            InitializeGridNode();
        }

        public Vector3Int GetGridSize()
        {
            if (!IsInitialized())
                return Vector3Int.zero;
            
            return gridSize;
        }

        public NodeBase[] GetGridNodes()
        {
            if (!IsInitialized())
                return null;
            
            return gridNodes;
        }

        public NodeBase GetNode(Vector3Int _pos)
        {
            var _node = GridUtility.GetNode(_pos, gridSize, gridNodes);

            if (!_node)
            {
                Debug.LogWarning($"Cannot find Node base at position {_pos}.");
                return null;
            }
            
            return _node;
        }

        public void InitializeGridNode()
        {
            if(IsInitialized()) return;
            
            Debug.Log($"GridNodeData start initializing...");

            isInit = true;
            
            gridSize = gridNodeDataSetting.GridSize;

            var _nodePrefab = gridNodeDataSetting.NodePrefab;

            if (!_nodePrefab)
            {
                _nodePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<NodeBase>();
                
                Debug.LogWarning($"GridNodeDataSetting Missing NodePrefab. Using Primitive Cube instead.");
            }
            
            gridNodes = GridUtility.GenerateGridINodes<NodeBase>(gridSize, _nodePrefab.gameObject);
        }

        public void ClearData()
        {
            foreach (var _node in gridNodes)
            {
                DestroyImmediate(_node.gameObject);
            }
            
            gridNodes = null;
            isInit    = false;
        }

        private bool IsInitialized()
        {
            if (!isInit)
            {
                Debug.LogWarning($"GridNodeData still not initialized");
                return false;
            }
            return true;
        }
    }
}
