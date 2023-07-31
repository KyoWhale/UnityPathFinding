using System;
using System.Collections.Generic;

public class MinHeap<T> where T : IComparable<T>
{
    List<T> _heap = new List<T>();

    public int Count
    {
        get => _heap.Count;
    }

    public void Push(T element)
    {
        _heap.Add(element);
        int current = Count - 1;

        while (current > 0)
        {
            int parent = (current-1) / 2;

            if (_heap[current].CompareTo(_heap[parent]) > 0)
                break;

            Swap(current, parent);
            current = parent;
        }
    }

    public T Pop()
    {
        if (Count == 0)
            return default(T);

        T temp = _heap[0];
        _heap[0] = _heap[Count-1];
        _heap.RemoveAt(Count-1);

        int current = 0;
        while (true)
        {
            int left = (current * 2) + 1;
            int right = (current * 2) + 2;
            if (_heap[current].CompareTo(_heap[left]) > 0)
            {
                Swap(current, left);
                current = left;
                continue;
            }
            if (_heap[current].CompareTo(_heap[right]) > 0)
            {
                Swap(current, right);
                current = right;
                continue;
            }
            
            return temp;
        }
    }

    private void Swap(int index1, int index2)
    {
        T temp = _heap[index1];
        _heap[index1] = _heap[index2];
        _heap[index2] = temp;
    }
}
