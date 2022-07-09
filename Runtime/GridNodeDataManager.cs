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
        [ReadOnly] 
        [SerializeField] private GridNodeData<NodeBase> gridNodeData;

        [SerializeField] private bool isInit;

        public GridNodeData<NodeBase> GetGridNodeData()
        {
            return gridNodeData;
        }

        public Vector3Int GetGridSize()
        {
            if (!IsInitialized())
                return Vector3Int.zero;
            
            return gridNodeData.GridSize;
        }

        public NodeBase[] GetGridNodes()
        {
            if (!IsInitialized())
                return null;
            
            return gridNodeData.GridNodes;
        }

        public NodeBase GetNode(Vector3Int _pos)
        {
            var _node = GridUtility.GetNode(_pos, gridNodeData.GridSize, gridNodeData.GridNodes);

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
            
            gridNodeData.GridSize = gridNodeDataSetting.GridSize;

            var _nodePrefab = gridNodeDataSetting.NodePrefab;

            if (!_nodePrefab)
            {
                _nodePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<NodeBase>();
                
                Debug.LogWarning($"GridNodeDataSetting Missing NodePrefab. Using Primitive Cube instead.");
            }
            
            gridNodeData.GridNodes = GridUtility.GenerateGridINodes<NodeBase>(gridNodeData.GridSize, _nodePrefab.gameObject);
        }

        public void ClearData()
        {
            foreach (var _node in gridNodeData.GridNodes)
            {
                DestroyImmediate(_node.gameObject);
            }
            
            gridNodeData.GridNodes = null;
            isInit                 = false;
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
