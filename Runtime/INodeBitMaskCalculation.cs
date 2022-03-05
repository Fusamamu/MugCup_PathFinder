using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public static class INodeBitMaskCalculation
    {
        private const int swBit = 0b_100_000_000;
        private const int nwBit = 0b_001_000_000;
        private const int neBit = 0b_000_000_001;
        private const int seBit = 0b_000_000_100;

        private const int wBit = 0b_010_000_000;
        private const int nBit = 0b_000_001_000;
        private const int eBit = 0b_000_000_010;
        private const int sBit = 0b_000_100_000;

        /// <summary>
        /// Get BitMask that calculated from surrounding node in 8 directions.
        /// All nodes are on the same plane/axis.
        /// </summary>
        /// <param name="_nodes">Expecting node list/array of 9. </param>
        /// <typeparam name="T">INode</typeparam>
        /// <returns></returns>
        public static int GetCalculatedBitMask<T>(IEnumerable<T> _nodes) where T: INode
        {
            var _bitMask  = 0b_0000_0_0000;
            var _startBit = 0b_1000_0_0000;

            foreach (var _node in _nodes)
            {
                if (_node != null)
                    _bitMask |= _startBit;

                _startBit >>= 1;
            }

            if (IsHaveBit(_bitMask, swBit))
            {
                var _someNeighborVoid = !IsHaveBit(_bitMask, wBit) || !IsHaveBit(_bitMask, sBit);
                    
                if (_someNeighborVoid)
                    _bitMask = RemoveBit(_bitMask, swBit);
            }
            
            if (IsHaveBit(_bitMask, nwBit))
            {
                var _someNeighborVoid = !IsHaveBit(_bitMask, wBit) || !IsHaveBit(_bitMask, nBit);
                    
                if (_someNeighborVoid)
                    _bitMask = RemoveBit(_bitMask, nwBit);
            }
            
            if (IsHaveBit(_bitMask, neBit))
            {
                var _someNeighborVoid = !IsHaveBit(_bitMask, nBit) || !IsHaveBit(_bitMask, eBit);
                    
                if (_someNeighborVoid)
                    _bitMask = RemoveBit(_bitMask, neBit);
            }
            
            if (IsHaveBit(_bitMask, seBit))
            {
                var _someNeighborVoid = !IsHaveBit(_bitMask, sBit) || !IsHaveBit(_bitMask, eBit);
                    
                if (_someNeighborVoid)
                    _bitMask = RemoveBit(_bitMask, seBit);
            }
            
            return _bitMask;
        }

        public static int GetNodeType(int _bitMask)
        {
            //Have 48 tile possibilities
            switch (_bitMask)
            {
                case 0b_111_111_111:
                    return 0;
                case 0b_000_011_000:
                    return 1;
                case 0b_010_010_000:
                    return 2;
                case 0b_010_011_000:
                    return 3;
                case 0b_011_011_000:
                    return 4;
                case 0b_000_010_010:
                    return 5;
                case 0b_000_011_010:
                    return 6;
                case 0b_000_011_011:
                    return 7;
                case 0b_010_010_010:
                    return 8;
                case 0b_010_011_010:
                    return 9;
                case 0b_011_011_010:
                    return 10;
                case 0b_010_011_011:
                    return 11;
                case 0b_011_011_011:
                    return 12;
                case 0b_000_110_000:
                    return 13;
                case 0b_000_111_000:
                    return 14;
                case 0b_010_110_000:
                    return 15;
                case 0b_010_111_000:
                    return 16;
                case 0b_011_111_000:
                    return 17;
                case 0b_000_110_010:
                    return 18;
                case 0b_000_111_010:
                    return 19;
                case 0b_000_111_011:
                    return 20;
                case 0b_010_110_010:
                    return 21;
                case 0b_010_111_010:
                    return 22;
                case 0b_011_111_110:
                    return 23;
                case 0b_010_111_011:
                    return 24;
                case 0b_011_111_011:
                    return 25;
                case 0b_110_110_000:
                    return 26;
                case 0b_110_111_000:
                    return 27;
                case 0b_111_111_000:
                    return 28;
                case 0b_110_110_010:
                    return 29;
                case 0b_110_111_011:
                    return 30;
                case 0b_111_111_010:
                    return 31;
                // case 0b_111_111_11:
                //     return 32;
                case 0b_111_111_011:
                    return 33;
                case 0b_000_110_110:
                    return 34;
                case 0b_000_111_110:
                    return 35;
                case 0b_000_111_111:
                    return 36;
                case 0b_010_110_110:
                    return 37;
                case 0b_010_111_110:
                    return 38;
                // case 0b_011_111_110:
                //     return 39;
                case 0b_010_111_111:
                    return 40;
                case 0b_011_111_111:
                    return 41;
                case 0b_110_110_110:
                    return 42;
                case 0b_110_111_110:
                    return 43;
                case 0b_111_111_110:
                    return 44;
                case 0b_110_111_111:
                    return 45;
                // case 0b_111_111_111:
                //     return 46;
                case 0b_000_010_000:
                    return 47;
                default:
                    return -1;
            }
        }

        private static int RemoveBit(int _originBit, int _targetBit)
        {
            return _originBit ^ _targetBit;
        }

        public static bool IsHaveBit(int _originBit, int _comparedBit)
        {
            return (_originBit & _comparedBit) == _comparedBit;
        }
    }
}
