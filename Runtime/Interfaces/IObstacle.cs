using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
	public interface IObstacle
	{
		public bool IsObstacle { get; }
	}
}
