﻿using System.Diagnostics;


namespace AlgorithmsLibrary
{
    /// <summary>
    /// Represents element of a heap.
    /// </summary>
    /// <typeparam name="TKey">Type of element key.</typeparam>
    /// <typeparam name="TValue">Type of element value.</typeparam>
    [DebuggerDisplay("{Key}: {Value}")]
    public sealed class HeapElement<TKey, TValue>
    {
        [DebuggerStepThrough]
        public HeapElement(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets element key.
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// Gets element value.
        /// </summary>
        public TValue Value { get; }
    }
    /// <summary>
    /// Represents heap with unique values.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public class UniqueHeap<TKey, TValue> : IHeap<TKey, TValue>
    {
        private readonly IComparer<TKey> _comparer;
        private readonly Dictionary<TValue, int> _valuesPositions = new Dictionary<TValue, int>();
        private HeapElement<TKey, TValue>[] _elements;

        public int Count { get; private set; }

        [DebuggerStepThrough]
        public UniqueHeap(IComparer<TKey> comparer)
            : this(comparer, 5)
        { }

        [DebuggerStepThrough]
        public UniqueHeap(IComparer<TKey> comparer, int initialCapacity)
        {
            if (initialCapacity < 1) throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Initial capacity must be positive.");
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            _comparer = comparer;
            _elements = new HeapElement<TKey, TValue>[initialCapacity];
        }

        [DebuggerStepThrough]
        public UniqueHeap(IComparer<TKey> comparer, HeapElement<TKey, TValue>[] initialHeap)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            if (initialHeap == null) throw new ArgumentNullException(nameof(initialHeap));
            _comparer = comparer;
            _elements = initialHeap;
            Count = _elements.Length;
            if (Count > 0)
            {
                Heapify();
            }
        }

        private void Heapify()
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                var element = _elements[i];

                SaveElementPosition(i, element);
            }

            for (int i = (Count / 2); i >= 0; i--)
            {
                RestoreHeap(i);
            }
        }

        private void SaveElementPosition(int position, HeapElement<TKey, TValue> element)
        {
            //if (_valuesPositions.ContainsKey(element.Value))
            //{ ThrowValueNotUniqueException(); }
            //else
            //{ 
                _valuesPositions[element.Value] = position;
            //}
        }

        [DebuggerStepThrough]
        public HeapElement<TKey, TValue> GetMinElement()
        {
            if (Count == 0)
                ThrowHeapEmptyException();

            return _elements[0];
        }

        private static void ThrowHeapEmptyException()
        {
            throw new InvalidOperationException("Heap is empty");
        }

        private static void ThrowValueNotUniqueException()
        {
            throw new InvalidOperationException("Heap contains not unique values");
        }

        public void Add(TKey key, TValue value)
        {
            UpdateStorageArray();

            var element = new HeapElement<TKey, TValue>(key, value);
            var position = Count++;

            _elements[position] = element;

            SaveElementPosition(position, element);

            UpdateHeapAfterInsert();
        }

        private void UpdateStorageArray()
        {
            if (_elements.Length > Count)
                return;

            var newElements = new HeapElement<TKey, TValue>[GetNewCapacity()];
            _elements.CopyTo(newElements, 0);
            _elements = newElements;
        }

        private int GetNewCapacity()
        {
            var newCapacity = Count + Count / 2;
            if (newCapacity == Count) newCapacity++;
            return newCapacity;
        }

        private void UpdateHeapAfterInsert()
        {
            var currentIndex = Count - 1;

            while (true)
            {
                if (currentIndex == 0)
                    return;

                var parentIndex = (currentIndex - 1) / 2;

                if (CorrectOrder(parentIndex, currentIndex))
                    return;

                SwapElements(parentIndex, currentIndex);

                currentIndex = parentIndex;
            }
        }

        private bool CorrectOrder(int parentIndex, int childIndex)
        {
            return _comparer.Compare(_elements[parentIndex].Key, _elements[childIndex].Key) <= 0;
        }

        private void SwapElements(int firstIndex, int secondIndex)
        {
            var firstElement = _elements[firstIndex];
            var secondElement = _elements[secondIndex];

            var tempElement = firstElement;
            _elements[firstIndex] = secondElement;
            _elements[secondIndex] = tempElement;

            _valuesPositions[firstElement.Value] = secondIndex;
            _valuesPositions[secondElement.Value] = firstIndex;
        }

        public HeapElement<TKey, TValue> ExtractMinElement()
        {
            if (Count == 0)
                ThrowHeapEmptyException();

            var minElement = _elements[0];
            var lastElement = _elements[Count - 1];

            _valuesPositions.Remove(minElement.Value);
            _valuesPositions[lastElement.Value] = 0;
            _elements[0] = lastElement;
            Count--;

            RestoreHeap(0);

            return minElement;
        }

        private void RestoreHeap(int index)
        {
            if (index >= Count)
                return;

            var leftChildIndex = 2 * index + 1;
            var rightChildIndex = 2 * index + 2;
            var largestElementIndex = index;

            if (leftChildIndex < Count && _comparer.Compare(_elements[leftChildIndex].Key, _elements[largestElementIndex].Key) < 0)
                largestElementIndex = leftChildIndex;
            if (rightChildIndex < Count && _comparer.Compare(_elements[rightChildIndex].Key, _elements[largestElementIndex].Key) < 0)
                largestElementIndex = rightChildIndex;

            if (largestElementIndex != index)
            {
                SwapElements(index, largestElementIndex);
                RestoreHeap(largestElementIndex);
            }
        }


        public bool Remove(TValue value)
        {
            int index;
            if (_valuesPositions.TryGetValue(value, out index) == false)
            { return false; }

            RemoveAt(index);
            return true;
        }

        private void RemoveAt(int index)
        {
            if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));

            _valuesPositions[_elements[Count - 1].Value] = index;
            _valuesPositions.Remove(_elements[index].Value);

            _elements[index] = _elements[Count - 1];
            Count--;

            RestoreHeap(index);
        }
    }
    /// <summary>
    /// Represents heap.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public interface IHeap<TKey, TValue>
    {
        /// <summary>
        /// Gets number of elements in the heap.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Extracts from the heap the element with the minimum key.
        /// </summary>
        /// <returns>Element with the minimum key.</returns>
        HeapElement<TKey, TValue> ExtractMinElement();
        /// <summary>
        /// Get from the heap the element with the minimum key without removing it from the heap.
        /// </summary>
        /// <returns>Element with the minimum key.</returns>
        HeapElement<TKey, TValue> GetMinElement();
        /// <summary>
        /// Inserts new element into the heap.
        /// </summary>
        /// <param name="key">Key of the element.</param>
        /// <param name="value">Value of the element.</param>
        void Add(TKey key, TValue value);
        /// <summary>
        /// Removes value from the heap.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        /// <returns>True, if value was removed from the heap, false otherwise.</returns>
        bool Remove(TValue value);
    }
}
