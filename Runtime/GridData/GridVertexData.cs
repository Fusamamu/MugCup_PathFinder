using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public class GridVertexData : GridData<VertexNode>
	{
		public override void ClearData()
		{
			if(GridNodes == null) return;
			
			foreach (var _vertex in GridNodes)
			{
				if(_vertex == null) continue;
				
				if (Application.isPlaying)
					Destroy(_vertex.gameObject);
				else
					DestroyImmediate(_vertex.gameObject);
			}
			
			base.ClearData();
		}
	}
}

