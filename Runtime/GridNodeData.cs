using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    //Need to Change to global manager for grid node
    public class GridNodeData : MonoBehaviour
    {
        [SerializeField] private GridNodeDataSetting gridNodeDataSetting;
        
        [SerializeField] private Vector3Int gridSize;
        [SerializeField] private NodeBase[] gridNodes;

        public Vector3Int GetGridSize()
        {
            return gridSize;
        }

        public NodeBase[] GetGridNodes()
        {
            return gridNodes;
        }
    }
}
