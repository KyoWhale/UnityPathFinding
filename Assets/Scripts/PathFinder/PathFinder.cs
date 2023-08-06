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
    public int x, y;
    public int cost = 0;
    public Node previous = null;

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

    public virtual int CompareTo(Node other)
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

    public override string ToString()
    {
        return $"({x},{y}) : {cost}\n";
    }
}

public abstract class Pathfinder : MonoBehaviour
{
    protected int mapWidth;
    protected int mapHeight;

    protected Node _startNode;
    protected Node _endNode;

    protected MinHeap<Node> _openNodes = new MinHeap<Node>();
    protected HashSet<Node> _closedNodes = new HashSet<Node>();

    private float waitingTime = 0.1f;
    protected WaitForSeconds _wait;

    public bool canDiagonalMove = true;
    protected int _directionLength;

    protected readonly int[] DIRECTION_X = {1, 0, -1, 0, 1, -1, -1, 1};
    protected readonly int[] DIRECTION_Y = {0, 1, 0, -1, 1, 1, -1, -1};
    public const int STRAIGHT_COST = 10;
    public const int DIAGONAL_COST = 14;

    private void Start()
    {
        _wait = new WaitForSeconds(waitingTime);
    }

    public void StartPathFinding()
    {
        SetNodes();
        StartCoroutine(FindPath());
    }

    protected virtual void SetNodes()
    {
        if (Map.instance == null || Map.instance.startCell == null || Map.instance.destinationCell == null)
        {
            Debug.LogError("맵 인스턴스 혹은 시작 셀이나 끝 셀을 올바르게 설정해주시기 바랍니다.");
            gameObject.SetActive(false);
            return;
        }

        mapWidth = Map.instance.width;
        mapHeight = Map.instance.height;
        _startNode = new Node(Map.instance.startCell.x, Map.instance.startCell.y);
        _endNode = new Node(Map.instance.destinationCell.x, Map.instance.destinationCell.y);
        _directionLength = canDiagonalMove ? 8 : 4;
    }

    private IEnumerator FindPath()
    {
        _openNodes.Add(_startNode);
        while(_openNodes.Count > 0)
        {
            Node currentNode = _openNodes.Pop();
            if (_closedNodes.TryGetValue(currentNode, out var actualValue) && currentNode.CompareTo(actualValue) >= 0)
            {
                continue;
            }

            SetCellInfo(currentNode, CellType.Searching);

            var newNodes = SearchNeighbor(currentNode);
            foreach (var node in newNodes)
            {
                if (node.x == _endNode.x && node.y == _endNode.y)
                {
                    IteratePath(node);
                    yield break;
                }
                _openNodes.Add(node);
                SetCellInfo(node, CellType.OpenList);
                yield return _wait;
            }

            _closedNodes.Add(currentNode);
            SetCellInfo(currentNode, CellType.Visited);
            yield return _wait;
        }
    }

    protected abstract List<Node> SearchNeighbor(Node current);
    private void IteratePath(Node endNode)
    {
        var currentNode = endNode;
        while (currentNode != null)
        {
            SetCellInfo(currentNode, CellType.Path);
            currentNode = currentNode.previous;
        }
    }

    private void SetCellInfo(Node node, CellType cellType)
    {
        if (Map.instance == null)
        {
            return;
        }
        Map.instance.SetCellType(node.x, node.y, cellType);
        Map.instance.SetCellText(node.x, node.y, node.ToString());
    }
}
