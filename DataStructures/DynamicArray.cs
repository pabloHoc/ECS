using System;
using System.Collections;
using System.Collections.Generic;

namespace ECS
{
public class DynamicArray<T> where T : IComponent
{
        private T[] items;
        private int initialSize;
        private int size;
        private float growFactor = 0.5f;

        public DynamicArray(int initialSize)
        {
            this.initialSize = initialSize;
            items = new T[initialSize];
        }

        public int Count
        {
            get
            {
                return size;
            }
        }
        public void AddAt(T item, uint index)
        {
            if (size + 1 > items.Length)
            {
                var newArray = new T[(int)(items.Length * growFactor)];
                items.CopyTo(newArray, 0);
                items = newArray;
            }
            items[index] = item;
            size++;
        }

        public ref T GetAt(uint index)
        {
            if (index >= items.Length)
                throw new IndexOutOfRangeException();

            return ref items[index];
        }

        public bool Contains(T item)
        {
            foreach(var value in items)
            {
                if (value.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool RemoveAt(uint index)
        {
            if (items[index] != null && !items[index].Equals(default(T)))
            {
                items[index] = default(T);
                return true;
            }
            return false;
        }
    }
}