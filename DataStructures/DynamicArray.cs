using System;
using System.Collections;
using System.Collections.Generic;

namespace ECS
{
public class DynamicArray<T> where T : IComponent
{
        private T[][] items;
        private int initialSize;
        private int size;
        private float growFactor = 0.5f;

        public DynamicArray(int initialSize)
        {
            this.initialSize = initialSize;
            items = new T[initialSize][];
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
                ResizeArray();

            var innerArray = items[index];

            if (innerArray == null) {
                items[index] = new T[1];
                items[index][0] = item;
                size++;
            } else {
                var innerArrayElementCount = GetInnerArrayElementCount(index);                

                if (innerArrayElementCount == innerArray.Length)
                    ResizeInnerArray(index, innerArrayElementCount);

                items[index][innerArrayElementCount] = item;

                if (innerArrayElementCount == 0)
                    size++;
            }
        }

        private int GetInnerArrayElementCount(uint index)
        {
            var innerArray = items[index];
            var count = 0;

            for (int i = 0; i < innerArray.Length; i++)
                if (!innerArray[i].Equals(default(T)))
                    count++;
            
            return count;                    
        }

        private void ResizeInnerArray(uint index, int currentSize)
        {
            var newArray = new T[(int)(currentSize * growFactor)];
            items[index].CopyTo(newArray, 0);
            items[index] = newArray;
        }

        private void ResizeArray()
        {
            var newArray = new T[(int)(items.Length * growFactor)][];
            items.CopyTo(newArray, 0);
            items = newArray;
        }

        public ref T[] GetAt(uint index)
        {
            if (index >= items.Length)
                throw new IndexOutOfRangeException();

            return ref items[index];
        }
        
        public ref T GetUnique(uint index)
        {
            if (index >= items.Length)
                throw new IndexOutOfRangeException();

            return ref items[index][0];
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

        public bool RemoveAllAt(uint index)
        {
            if (items[index] != null) {
                items[index] = null;
                return true;
            }               
            return false;      
        }

        public bool RemoveAt(uint index, T item)
        {
            for (int i = 0; i < items[index].Length; i++)
            {
                if (items[index][i].Equals(item)) {
                    items[index][i] = default(T);
                    return true;
                }
            }
            return false;
        }

        private void ShrinkInnerArray(uint index) 
        {
            var newSize = GetInnerArrayElementCount(index);
            var newArray = new T[newSize];
            var j = 0;

            for (int i = 0; i < items[index].Length; i++)
            {
                if (!items[index][i].Equals(default(T)))
                {
                    newArray[j] = items[index][i];
                    j++;
                }
            }

            items[index] = newArray;
        }
    }
}