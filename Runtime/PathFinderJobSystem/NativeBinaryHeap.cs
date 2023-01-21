using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace MugCup_PathFinder.Runtime
{
    public struct NativeBinaryHeap<T> : IDisposable where T : unmanaged, IComparable<T>, IEquatable<T>
    {
	    public int CurrentItemCount { get; private set; }
	    public int Capacity         { get; private set; }

	    public T this[int _i] => Items[_i];

	    public NativeArray<T>        Items;
		public NativeHashMap<T, int> ItemIndices;

		public NativeBinaryHeap (int _maxHeapSize, Allocator _allocator) 
		{
			Items       = new NativeArray<T>(_maxHeapSize, _allocator, NativeArrayOptions.UninitializedMemory);
			ItemIndices = new NativeHashMap<T, int>(128, _allocator);
			
			CurrentItemCount = 0;
			Capacity         = _maxHeapSize;
		}

		public void Add(T _item) 
		{
			UpdateHeapItem(_item, CurrentItemCount);
			SortUp(_item);
			CurrentItemCount++;
		}

		public T RemoveFirst() 
		{
			T _firstItem = Items[0];
			
			CurrentItemCount--;
			
			var _item = Items[CurrentItemCount];
			
			UpdateHeapItem(_item, 0);
			SortDown(_item);

			return _firstItem;
		}

		public T RemoveAt(int _index) 
		{
			T _firstItem = Items[_index];
			
			CurrentItemCount--;
			
			if (_index == CurrentItemCount) 
			{
				return _firstItem;
			}

			var _item = Items[CurrentItemCount];
			
			UpdateHeapItem(_item, _index);
			SortDown(_item);

			return _firstItem;
		}

		public int IndexOf(T _item) 
		{
			return GetHeapIndex(_item);
		}

		private void SortDown(T _item) 
		{
			while (true) 
			{
				int _itemIndex       = GetHeapIndex(_item);
				int _childIndexLeft  = _itemIndex * 2 + 1;
				int _childIndexRight = _itemIndex * 2 + 2;

				if (_childIndexLeft < CurrentItemCount) 
				{
					var _swapIndex = _childIndexLeft;

					if (_childIndexRight < CurrentItemCount) 
					{
						if (Items[_childIndexLeft].CompareTo(Items[_childIndexRight]) < 0) 
						{
							_swapIndex = _childIndexRight;
						}
					}

					if (_item.CompareTo(Items[_swapIndex]) < 0) 
					{
						Swap(_item, Items[_swapIndex]);
					} 
					else 
					{
						return;
					}

				} else 
				{
					return;
				}
			}
		}

		private void SortUp (T _item) 
		{
			int _parentIndex = (GetHeapIndex(_item) - 1) / 2;

			while (true) 
			{
				T _parentItem = Items[_parentIndex];
				
				if (_item.CompareTo(_parentItem) > 0) 
				{
					Swap(_item, _parentItem);
				} 
				else 
				{
					break;
				}

				_parentIndex = (GetHeapIndex(_item) - 1) / 2;
			}
		}

		private void Swap(T _itemA, T _itemB) 
		{
			int _itemAIndex = GetHeapIndex(_itemA);
			int _itemBIndex = GetHeapIndex(_itemB);

			UpdateHeapItem(_itemB, _itemAIndex);
			UpdateHeapItem(_itemA, _itemBIndex);
		}

		private void UpdateHeapItem(T _item, int _newIndex) 
		{
			ItemIndices.Remove(_item);
			bool _success = ItemIndices.TryAdd(_item, _newIndex);
			Items[_newIndex] = _item;
		}

		private int GetHeapIndex(T _item) 
		{
			if (ItemIndices.TryGetValue(_item, out int _result)) 
			{
				return _result;
			}
			return -1;
		}

		public void Dispose () 
		{
			Items.Dispose();
			ItemIndices.Dispose();
		}
    }
}
