using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MugCup_PathFinder.Runtime;

namespace MugCup_PathFinder.Tests.Editor
{
    public class grid_bound_check
    {
        [Test]
        public void grid_bound_checkSimplePasses()
        {
            bool _insideGridBound = GridUtility.NodePositionInsideGrid(new Vector3Int(1, 1, 1), new Vector3Int(3, 3, 3));
            
            Assert.AreEqual(true, _insideGridBound);
        }
    }
}
