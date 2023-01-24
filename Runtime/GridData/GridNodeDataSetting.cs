using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    [CreateAssetMenu(fileName = "GridNodeDataSettingSO", menuName = "ScriptableObjects/Mugcup Path Finder/GridNodeDataSetting", order = 1)]
    public class GridNodeDataSetting : ScriptableObject
    {
        public PathAgent      PathAgentPrefab;
        public GridNode   GridNodePrefab;
        public Vector3Int GridSize;
    }
}
