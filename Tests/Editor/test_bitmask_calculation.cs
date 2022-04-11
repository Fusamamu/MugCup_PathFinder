using System.Collections;
using System.Collections.Generic;
using MugCup_PathFinder.Runtime;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

public class test_bitmask_calculation
{
    private class TestNode : INode
    {
        public INode NodeParent {
            get;
            set;
        }
        public Vector3Int NodePosition {
            get;
            set;
        }
        public int G_Cost {
            get;
            set;
        }
        public int H_Cost {
            get;
            set;
        }
        public int F_Cost {
            get;
        }
    }
    
    [Test]
    public void test_bitmask_calculationSimplePasses()
    {
        var _selectedPos = new Vector3Int(1, 0, 1);
        var _gridSize    = new Vector3Int(3, 1, 3);
    
        var _row    = _gridSize.z;
        var _column = _gridSize.x;
        
        var _testGrids = new TestNode[_row * _column];
    
        for (var _i = 0; _i < _testGrids.Length; _i++)
        {
            _testGrids[_i] = new TestNode();
        }
    
        _testGrids[0] = null;
        _testGrids[3] = null;
    
        var _surroundingNodes  = GridUtility.GetMiddleSectionNodesFrom3x3Cube(_selectedPos, _gridSize, _testGrids);
    
        var _calculatedBitMask = BitMaskCalculation.GetCalculatedBitMask(_surroundingNodes);

        var _expectedBitMask = 0b_011_011_011;
        
        Assert.AreEqual(_expectedBitMask, _calculatedBitMask);
    }

    [Test] 
    public void test_bit_operation_methods()
    {
        var _checkedBit  = 0b_100_011_000;
        var _comparedBit = 0b_100_000_000;

        Assert.True(BitMaskCalculation.IsHaveBit(_checkedBit, _comparedBit));
    }
}
