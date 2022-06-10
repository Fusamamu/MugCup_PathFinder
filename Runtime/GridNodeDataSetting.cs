using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    [CreateAssetMenu(fileName = "GridNodeDataSettingSO", menuName = "ScriptableObjects/Mugcup Path Finder/GridNodeDataSetting", order = 1)]
    public class GridNodeDataSetting : ScriptableObject
    {
        public Vector3Int GridSize;
    }
}
