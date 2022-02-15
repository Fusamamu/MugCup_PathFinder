using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MugCup_PathFinder.Runtime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MugCup_PathFinder.Tests.Editor
{
    public class grid_area_selection_check
    {
        [Test]
        public void Check_CrossNodes_Count()
        {
            var _selectedPosition = Vector3Int.zero;
            var _range            = Random.Range(1, 10);
            
            var _crossNodePositions = GridAreaSelection.GetCrossNodesXZ(_selectedPosition, _range);
            var _positionCount      = _crossNodePositions.Count();
            
            var _expectCount = _range * 4;
            
            Debug.Log($"Range : {_range}. Expected Count : { _expectCount}. Position Count : {_positionCount}");
            
            Assert.AreEqual(_expectCount, _positionCount);
        }
    }
}
