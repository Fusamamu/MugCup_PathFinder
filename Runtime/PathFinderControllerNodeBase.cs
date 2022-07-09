using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public class PathFinderControllerNodeBase : PathFinderControllerGeneric<NodeBase>
    {
        [SerializeField] private bool useGridNodeDataManager;
        
        [SerializeField] private GridNodeDataManager gridNodeDataManager;
        
        public void SetUseGridNodeDataManager(bool _value)
        {
            useGridNodeDataManager = _value;
        }
        
        public override void Initialized(GridNodeData<NodeBase> _gridNodeData = null)
        {
            if(useGridNodeDataManager)
                InjectGridNodeDataManager(gridNodeDataManager);
            else
                base.Initialized(gridNodeData);
        }
    
        private void InjectGridNodeDataManager(GridNodeDataManager _gridNodeDataManager = null)
        {
            /*Using GridNodeData from GridNodeDataManager this is out of the box data from Path Finder Package*/
            gridNodeDataManager = _gridNodeDataManager != null ? _gridNodeDataManager : FindObjectOfType<GridNodeDataManager>();
        
            if (!gridNodeDataManager)
            {
                Debug.LogWarning($"GridNodeData Missing Reference.");
                return;
            }
        
            gridNodeData.GridSize  = gridNodeDataManager.GetGridSize ();
            gridNodeData.GridNodes = gridNodeDataManager.GetGridNodes();
        }
    }
}
