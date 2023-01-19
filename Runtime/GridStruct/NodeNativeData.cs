using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public struct NodeNativeData
    {
        public int3 GridPos;
        public int3 NextNodePos;
        public int3 PrevNodePos;
        
        public float3 WorldPos;
        public float3 NextWorldPos;
        public float3 PrevWorldPos;

        public bool IsObstacle;
    }
}
