using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : Node
{
    private int _gCost = 0; // 현재 노드까지의 이동 거리
    private int _hCost = 0; // 휴리스틱 값

    public int gCost
    {
        get
        {
            return _gCost;
        }
        set
        {
            _gCost = value;
            cost = _hCost + _gCost;
        }
    }

    public int hCost
    {
        get
        {
            return _hCost;
        }
        set
        {
            _hCost = value;
            cost = _hCost + _gCost;
        }
    }

    public AStarNode(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public AStarNode(int x, int y, int gCost, int hCost, AStarNode previous)
    {
        this.x = x;
        this.y = y;
        this.gCost = gCost;
        this.hCost = hCost;
        this.previous = previous;
    }

    public override int CompareTo(Node other)
    {
        AStarNode astarOtherNode = other as AStarNode;
        int result = cost - other.cost;
        if (astarOtherNode == null)
        {
            return result;
        }

        if (result == 0)
        {
            return hCost - astarOtherNode.hCost;
        }
        else
        {
            return result;
        }
    }

    public override string ToString()
    {
        return $"{gCost} {hCost}\n{cost}";
    }
}

public class AStar : Pathfinder
{
    protected override void SetNodes()
    {
        base.SetNodes();
        _startNode = new AStarNode(_startNode.x, _startNode.y, 0, CalculateHeuristic(_startNode.x, _startNode.y), null);
        _endNode = new AStarNode(_endNode.x, _endNode.y, 0, CalculateHeuristic(_endNode.x, _endNode.y), null);
    }

    protected override List<Node> SearchNeighbor(Node current)
    {
        List<Node> newNodes = new List<Node>();

        for (int i = 0; i < _directionLength; ++i)
        {
            int newX = current.x + DIRECTION_X[i];
            int newY = current.y + DIRECTION_Y[i];
            if (newX < 0 || mapWidth <= newX || newY < 0 || mapHeight <= newY)
            {
                continue;
            }
            if (Map.instance.IsObstacle(newX, newY))
            {
                continue;
            }

            int gCost = (current as AStarNode).gCost + (i < 4 ? STRAIGHT_COST : DIAGONAL_COST);
            int hCost = CalculateHeuristic(newX, newY);
            AStarNode newNode = new AStarNode(newX, newY, gCost, hCost, current as AStarNode);
            if (_closedNodes.TryGetValue(newNode, out var actualValue) && newNode.CompareTo(actualValue) >= 0)
            {
                continue;
            }
            newNodes.Add(newNode);
        }

        return newNodes;
    }

    private int CalculateHeuristic(int x, int y)
    {
        int dx = Math.Abs(x - _endNode.x);
        int dy = Math.Abs(y - _endNode.y);
        return  Math.Min(dx, dy) * DIAGONAL_COST + Math.Abs(dx - dy) * STRAIGHT_COST;
    }
}
