using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MugCup_PathFinder.Runtime
{
    public class PathFinderJobSystem : MonoBehaviour, IPathFinderController<GridNode>
    {
	    private bool isInit;
	    
	    [SerializeField] private GridData<GridNode> GridData;
	    
        private GridStructure gridStructure;
	    
        private PathRequest<Vector3Int> currentRequest;
        private readonly Queue<PathRequest<Vector3Int>> requests = new Queue<PathRequest<Vector3Int>>();
        
        private ProcessPathJob job;
        private JobHandle jobHandle;
        
        private int framesProcessed;

        public IPathFinderController<GridNode> SetGridData(GridData<GridNode> _gridData)
		{
			GridData = _gridData;
			
			gridStructure.Dispose();
			gridStructure = GridData.CopyData();
			
			return this;
		}
		
		public IPathFinderController<GridNode> SetPathFinder()
		{
			return this;
		}
		
		public void Initialized()
		{
			if(isInit) return;
			isInit = true;
		}

		public void RequestPath(PathRequest<Vector3Int> _request, bool _waitForComplete = false)
		{
			requests.Enqueue(_request);
		}

		private void Update () 
		{
			framesProcessed++;

			if (currentRequest != null) 
			{
				if (jobHandle.IsCompleted || framesProcessed > 3) 
				{
					jobHandle.Complete();

					if (job.Result.Length == 0)// || Vector3.Distance(currentRequest.PathEnd, job.GridStructure.GetNodePosition(job.Result[0])) > 3) 
					{
						currentRequest.Callback(null, false);
					} 
					else
					{
						var _path = new Vector3Int[job.Result.Length];

						for (var _i = job.Result.Length - 1; _i >= 0; _i--) 
							_path[_i] = job.Result[_i].AsVector3Int();

						currentRequest.Callback(_path, true);
					}

					job.ForceDispose();
					currentRequest = null;
				}
			}

			//Queue a new job if there are requests
			if (currentRequest == null && requests.Count > 0)//&& this.grid.NodeSize > 0) 
			{
				currentRequest = requests.Dequeue();
				
				job = new ProcessPathJob
				{
					StartNodePos  = currentRequest.PathStart.AsInt3(),
					TargetNodePos = currentRequest.PathEnd  .AsInt3(),
					
					GridStructure  = gridStructure.Copy(),
					Result         = new NativeList<int3>(Allocator.TempJob),
					Open           = new NativeBinaryHeap<NodeCost>(gridStructure.NodeCount, Allocator.TempJob),
					Closed         = new NativeHashMap<int3,NodeCost>(128, Allocator.TempJob)
				};
				
				jobHandle = job.Schedule();
				
				framesProcessed = 0;
			}
		}

		private void OnDestroy () 
		{
			jobHandle.Complete();
			
			job.Dispose();
			gridStructure.Dispose();
		}
    }
}
