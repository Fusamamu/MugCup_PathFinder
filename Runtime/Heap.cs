using UnityEngine;
using System.Collections;
using System;

namespace MugCup_PathFinder.Runtime
{
	public class Heap<T> where T : IHeapItem<T> 
	{
		public int Count => currentItemCount;
		
		private readonly T[] items;
		
		private int currentItemCount;
		
		public Heap(int _maxHeapSize) 
		{
			items = new T[_maxHeapSize];
		}
		
		public void Add(T _item) 
		{
			_item.HeapIndex = currentItemCount;
			
			items[currentItemCount] = _item;
			
			SortUp(_item);
			
			currentItemCount++;
		}

		public T RemoveFirst() 
		{
			T _firstItem = items[0];
			
			currentItemCount--;
			
			items[0] = items[currentItemCount];
			items[0].HeapIndex = 0;
			
			SortDown(items[0]);
			
			return _firstItem;
		}

		public void UpdateItem(T _item) 
		{
			SortUp(_item);
		}

		public bool Contains(T _item) 
		{
			return Equals(items[_item.HeapIndex], _item);
		}

		private void SortDown(T _item) 
		{
			while (true) 
			{
				int _childIndexLeft  = _item.HeapIndex * 2 + 1;
				int _childIndexRight = _item.HeapIndex * 2 + 2;

				if (_childIndexLeft < currentItemCount) 
				{
					var _swapIndex = _childIndexLeft;

					if (_childIndexRight < currentItemCount)
					{
						var _itemLeft  = items[_childIndexLeft ];
						var _itemRight = items[_childIndexRight];
						
						if (_itemLeft.CompareTo(_itemRight) < 0) 
						{
							_swapIndex = _childIndexRight;
						}
					}

					if (_item.CompareTo(items[_swapIndex]) < 0) 
						Swap (_item, items[_swapIndex]);
					else 
						return;
				}
				else 
				{
					return;
				}
			}
		}
		
		private void SortUp(T _item) 
		{
			int _parentIndex = (_item.HeapIndex - 1) / 2;
			
			while (true) 
			{
				T _parentItem = items[_parentIndex];
				
				if (_item.CompareTo(_parentItem) > 0) 
					Swap (_item,_parentItem);
				else 
					break;

				_parentIndex = (_item.HeapIndex -1 ) / 2;
			}
		}
		
		private void Swap(T _itemA, T _itemB) 
		{
			items[_itemA.HeapIndex] = _itemB;
			items[_itemB.HeapIndex] = _itemA;
			
			(_itemA.HeapIndex, _itemB.HeapIndex) = (_itemB.HeapIndex, _itemA.HeapIndex);
		}
	}

	public interface IHeapItem<T> : IComparable<T> 
	{
		int HeapIndex { get; set; }
	}
}
