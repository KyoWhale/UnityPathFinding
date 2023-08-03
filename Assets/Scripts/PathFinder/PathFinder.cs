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

    public Node() { }

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Node(int x, int y, int cost, Node previous)
    {
        this.x = x;
        this.y = y;
        this.cost = cost;
        this.previous = previous;
    }

    public int CompareTo(Node other)
    {
        return cost - other.cost;
    }

    public override bool Equals(object obj)
    {
        Node other = obj as Node;
        if (other == null)
        {
            return false;
        }
        return other.x == x && other.y == y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
}

public abstract class Pathfinder : MonoBehaviour
{
    protected Node _startNode;
    protected Node _endNode;

    protected MinHeap<Node> _openNodes = new MinHeap<Node>();
    protected HashSet<Node> _visitedNodes = new HashSet<Node>();

    private float waitingTime = 0.1f;
    protected WaitForSeconds _wait;

    private void Start()
    {
        _wait = new WaitForSeconds(waitingTime);
    }

    public void StartPathFinding()
    {
        SetNodes();
        StartCoroutine(FindPath());
    }

    private void SetNodes()
    {
        if (Map.instance == null || Map.instance.startCell == null || Map.instance.destinationCell == null)
        {
            Debug.LogError("No start or destination node");
            _startNode = new Node(0, 0);
            _endNode = new Node(Map.instance.width - 1, Map.instance.height - 1);
            return;
        }
        _startNode = new Node(Map.instance.startCell.x, Map.instance.startCell.y);
        _endNode = new Node(Map.instance.destinationCell.x, Map.instance.destinationCell.y);
        Debug.Log(GetType().ToString() + _startNode.x + ", " + _startNode.y);
    }

    private IEnumerator FindPath()
    {
        _openNodes.Push(_startNode);
        while(_openNodes.Count > 0)
        {
            Node currentNode = _openNodes.Pop();
            if (_visitedNodes.Contains(currentNode))
            {
                continue;
            }
            _visitedNodes.Add(currentNode);

            var newNodes = SearchNeighbor(currentNode);
            foreach (var node in newNodes)
            {
                if (node.x == _endNode.x && node.y == _endNode.y)
                {
                    IteratePath(node);
                    yield break;
                }
                _openNodes.Push(node);
                Map.instance.SetCellType(node.x, node.y, CellType.OpenList);
                yield return _wait;
            }

            Map.instance.SetCellType(currentNode.x, currentNode.y, CellType.Visited);
            yield return _wait;
        }
    }

    protected abstract List<Node> SearchNeighbor(Node current);
    private void IteratePath(Node endNode)
    {
        var current = endNode;
        while (current != null)
        {
            Map.instance.SetCellType(current.x, current.y, CellType.Complete);
            current = current.previous;
        }
    }
}
