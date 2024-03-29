using System.Collections;
using System.Collections.Generic;
using MugCup_PathFinder.Runtime;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

public class test_bitmask_calculation
{
    // private class TestNode : INode
    // {
    //     public INode NodeParent {
    //         get;
    //         set;
    //     }
    //     public Vector3Int NodeGridPosition {
    //         get;
    //         set;
    //     }
    //     public Vector3 NodeWorldPosition {
    //         get; 
    //         set;
    //     }
    //     public INode SetNodePosition(Vector3Int _nodePosition)
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //     public INode SetNodeWorldPosition(Vector3 _worldPosition)
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //
    //     public Vector3Int NextNodePosition {
    //         get; 
    //         set;
    //     }
    //     
    //     public Vector3 ExitPosition {
    //         get; 
    //         set;
    //     }
    //     
    //     public int G_Cost {
    //         get;
    //         set;
    //     }
    //     public int H_Cost {
    //         get;
    //         set;
    //     }
    //     public int F_Cost {
    //         get;
    //     }
    //
    //     public HashSet<INode> Neighbors {
    //         get;
    //     }
    //     public IEnumerable<T> GetNeighbors<T>() where T : INode
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //     public void SetNeighbors<T>(IEnumerable<T> _neighbors) where T : INode
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //     
    //     public INode NorthNode { get; }
    //     public INode SouthNode { get; }
    //     public INode WestNode  { get; }
    //     public INode EastNode  { get; }
    //     
    //     public INode NextNodeOnPath { get; set; }
    //
    //     public NodeDirection Direction { get; set; }
    //     
    //     public void SetNextNodeOnPath(INode _node)
    //     {
		  //   
    //     }
    //     public void SetNodePathDirection(NodeDirection _direction)
    //     {
		  //   
    //     }
    //     
    //     public INode GrowPathTo(INode _neighbor, NodeDirection _direction)
    //     {
    //         return default;
    //     }
    // }
    
    [Test]
    public void test_bitmask_calculationSimplePasses()
    {
        // var _selectedPos = new Vector3Int(1, 0, 1);
        // var _gridSize    = new Vector3Int(3, 1, 3);
        //
        // var _row    = _gridSize.z;
        // var _column = _gridSize.x;
        //
        // var _testGrids = new TestNode[_row * _column];
        //
        // for (var _i = 0; _i < _testGrids.Length; _i++)
        // {
        //     _testGrids[_i] = new TestNode();
        // }
        //
        // _testGrids[0] = null;
        // _testGrids[3] = null;
        //
        // var _surroundingNodes  = GridUtility.GetMiddleSectionNodesFrom3x3Cube(_selectedPos, _gridSize, _testGrids);
        //
        // var _calculatedBitMask = BitMaskCalculation.GetCalculatedBitMask(_surroundingNodes);
        //
        // var _expectedBitMask = 0b_011_011_011;
        //
        // Assert.AreEqual(_expectedBitMask, _calculatedBitMask);
    }

    [Test] 
    public void test_bit_operation_methods()
    {
        var _checkedBit  = 0b_100_011_000;
        var _comparedBit = 0b_100_000_000;

        Assert.True(BitMaskCalculation.IsHaveBit(_checkedBit, _comparedBit));
    }
}
