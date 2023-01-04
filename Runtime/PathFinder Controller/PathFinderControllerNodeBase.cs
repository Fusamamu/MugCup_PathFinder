using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    /// <summary>
    /// Inherit PathFinderControllerGeneric so that it can be attached to gameObject.
    /// </summary>
    public class PathFinderControllerNodeBase : PathFinderControllerGeneric<GridNode>
    {
        [SerializeField] private bool useGridNodeDataManager;
        
        [SerializeField] private GridNodeDataManager gridNodeDataManager;
        
        public void SetUseGridNodeDataManager(bool _value)
        {
            useGridNodeDataManager = _value;
        }
        
        public override void Initialized()
        {
            if(useGridNodeDataManager)
                InjectGridNodeDataManager(gridNodeDataManager);
            else
                base.Initialized();
        }
    
        private void InjectGridNodeDataManager(GridNodeDataManager _gridNodeDataManager = null)
        {
            /*Using GridNodeData from GridNodeDataManager this is out of the box data from Path Finder Package*/
            gridNodeDataManager = 
                _gridNodeDataManager != null ? 
                    _gridNodeDataManager : FindObjectOfType<GridNodeDataManager>();
        
            if (!gridNodeDataManager)
            {
                Debug.LogWarning($"GridNodeData Missing Reference.");
                return;
            }
            
            SelectGridDataNode(gridNodeDataManager.GetGridNodeData());
        }
    }
}
