using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;

public class MinHeap<T> where T : IComparable<T>
{
    List<T> _heap = new List<T>();

    public int Count
    {
        get => _heap.Count;
    }

    public void Add(T element)
    {
        Push(element);
    }

    public T Peak()
    {
        if (Count == 0)
        {
            return default(T);
        }
        return _heap[0];
    }

    public void Push(T element)
    {
        _heap.Add(element);
        int current = Count - 1;

        while (current > 0)
        {
            int parent = (current-1) / 2;

            if (_heap[current].CompareTo(_heap[parent]) >= 0)
            {
                break;
            }

            Swap(current, parent);
            current = parent;
        }
    }

    public T Pop()
    {
        if (Count == 0)
        {
            return default(T);
        }

        int count = Count; // 캐싱
        T temp = _heap[0];
        _heap[0] = _heap[count-1];
        _heap.RemoveAt(count-1);
        --count;

        int current = 0;
        while (true)
        {
            int left = (current * 2) + 1;
            int right = (current * 2) + 2;
            int next = current;
            if (left < count && _heap[next].CompareTo(_heap[left]) >= 0)
            {
                next = left;
            }
            if (right < count && _heap[next].CompareTo(_heap[right]) >= 0)
            {
                next = right;
            }
            if (next == current)
            {
                break;
            }

            Swap(current, next);
            current = next;
        }

        return temp;
    }

    private void Swap(int index1, int index2)
    {
        T temp = _heap[index1];
        _heap[index1] = _heap[index2];
        _heap[index2] = temp;
    }
}