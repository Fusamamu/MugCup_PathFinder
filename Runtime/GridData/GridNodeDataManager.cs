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
        [SerializeField] private GridData<GridNode> GridData;

        [SerializeField] private bool isInit;

        public GridData<GridNode> GetGridNodeData()
        {
            return GridData;
        }

        public Vector3Int GetGridSize()
        {
            if (!IsInitialized())
                return Vector3Int.zero;
            
            return GridData.GridSize;
        }

        public GridNode[] GetGridNodes()
        {
            if (!IsInitialized())
                return null;
            
            return GridData.GridNodes;
        }

        public GridNode GetNode(Vector3Int _pos)
        {
            var _node = GridUtility.GetNode(_pos, GridData.GridSize, GridData.GridNodes);

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
            
            GridData.GridSize = gridNodeDataSetting.GridSize;

            var _nodePrefab = gridNodeDataSetting.GridNodePrefab;

            if (!_nodePrefab)
            {
                _nodePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<GridNode>();
                
                Debug.LogWarning($"GridNodeDataSetting Missing NodePrefab. Using Primitive Cube instead.");
            }
            
            GridData.GridNodes = GridUtility.GenerateGridINodes<GridNode>(GridData.GridSize, _nodePrefab.gameObject);
        }

        public void ClearData()
        {
            foreach (var _node in GridData.GridNodes)
            {
                DestroyImmediate(_node.gameObject);
            }
            
            GridData.GridNodes = null;
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
