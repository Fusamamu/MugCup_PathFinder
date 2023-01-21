using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MugCup_PathFinder.Runtime
{
    public class PathFinderJobSystem : MonoBehaviour
    {
        private readonly Queue<PathRequest<Vector3Int>> requests = new Queue<PathRequest<Vector3Int>>();
        
        private PathRequest<Vector3Int> currentRequest;
        
        private ProcessPathJob job;
        private JobHandle jobHandle;
        
        private int framesProcessed = 0;

        private GridStructure grid;
        
		private void Update () 
		{
			framesProcessed++;

			if (currentRequest != null) 
			{
				if (jobHandle.IsCompleted || framesProcessed > 3) 
				{
					jobHandle.Complete();

					var _path = new PathResult<Vector3Int>();

					if (job.Result.Length == 0)// || Vector3.Distance(currentRequest.PathEnd, job.GridStructure.GetNodePosition(job.Result[0])) > 3) 
					{
						//_path.Success = false;
					} 
					else 
					{
						//_path.nodes = new List<Vector3>(job.Result.Length);

						for (var _i = job.Result.Length - 1; _i >= 0; _i--) 
						{
							//_path.nodes.Add(job.GridStructure.GetNodePosition(job.Result[_i].x, job.Result[_i].y));
						}
					}

					// currentRequest.result = path;
					// currentRequest.done = true;

					job.GridStructure.Dispose();
					job.Result       .Dispose();
					job.Open         .Dispose();
					job.Closed       .Dispose();
					
					currentRequest = null;
				}
			}

			//Queue a new job if there are requests
			if (currentRequest == null && requests.Count > 0)//&& this.grid.NodeSize > 0) 
			{
				currentRequest = requests.Dequeue();
				
				job = new ProcessPathJob
				{
					StartNodePos  = Vec3IntToInt3(currentRequest.PathStart),
					TargetNodePos = Vec3IntToInt3(currentRequest.PathEnd),
					
					GridStructure  = grid.Copy(),
					Result         = new NativeList<int3>(Allocator.TempJob),
					Open           = new NativeBinaryHeap<NodeCost>(grid.NodeCount, Allocator.TempJob),
					Closed         = new NativeHashMap<int3,NodeCost>(128, Allocator.TempJob)
				};
				
				jobHandle = job.Schedule();
				
				framesProcessed = 0;
			}
		}

		private int3 Vec3IntToInt3(Vector3Int _vector3)
		{
			return new int3(_vector3.x, _vector3.y, _vector3.z);
		}

		public void QueueJob(PathRequest<Vector3Int> _request) 
		{
			requests.Enqueue(_request);
		}

		public void UpdateGrid (GridStructure _grid) 
		{
			grid.Dispose();
			if (grid.NodeSize > 0) 
			{
				grid = _grid;
			}	
		}

		private void OnDestroy () 
		{
			jobHandle.Complete();
			job.GridStructure.Dispose();

			if (job.Result.IsCreated)
				job.Result.Dispose();
			
			if (job.Open.Items.IsCreated)
				job.Open.Dispose();
			
			if (job.Closed.IsCreated)
				job.Closed.Dispose();

			grid.Dispose();
		}
    }
}
