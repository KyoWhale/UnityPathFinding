using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    LeftUp, Left, LeftDown, Down, RightDown, Right, RightUp, Up
}

public class Node : IComparable<Node>
{
    public Node previous;
    public int x, y;
    public int cost;
    public Direction direction; // Direction from previous

    public int CompareTo(Node other)
    {
        return cost - other.cost;
    }
}

public abstract class PathFinder : MonoBehaviour
{
    public bool autoStart = true;

    public Vector2Int start;
    public Vector2Int end;
    protected Node _start = new Node();
    protected Node _current = new Node();
    protected Node _end = new Node();

    protected MinHeap<Node> _openNodes = new MinHeap<Node>();

    public float waitingTime;
    protected WaitForSeconds _wait;

    private void Start()
    {
        _wait = new WaitForSeconds(waitingTime);

        if (autoStart)
        {
            StartPathFinding();
        }
    }

    public void StartPathFinding()
    {
        SetNodes();
        StartCoroutine(FindPath());
    }

    private void SetNodes()
    {
        _start.x = start.x;
        _start.y = start.y;
        _end.x = end.x;
        _end.y = end.y;
    }

    private IEnumerator FindPath()
    {
        _openNodes.Push(_start);
        while(_openNodes.Count > 0)
        {
            _current = _openNodes.Pop();
            if (_current.x == _end.x && _current.y == _end.y)
            {
                _end = _current;
                break;
            }

            var newNodes = SearchFrom(_current);
            foreach (var node in newNodes)
            {
                _openNodes.Push(node);
                yield return _wait;
            }

            yield return _wait;
        }

        if (_current != _end)
        {
            Debug.LogError("Can not reach to destination");
            yield break;
        }
    }

    protected abstract List<Node> SearchFrom(Node current);
    protected abstract void CalculateCost(Node current);
}
