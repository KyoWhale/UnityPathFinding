using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : PathFinder
{
    protected override void CalculateCost(Node node)
    {
        if (node.previous == null)
        {
            node.cost = 1;
            return;
        }

        switch (node.direction)
        {
            case Direction.Down:
            case Direction.Up:
            case Direction.Left:
            case Direction.Right:
                node.cost = node.previous.cost + 10;
                break;
            case Direction.LeftDown:
            case Direction.LeftUp:
            case Direction.RightDown:
            case Direction.RightUp:
                node.cost = node.previous.cost + 14;
                break;
        }
    }

    protected override List<Node> SearchFrom(Node current)
    {
        throw new System.NotImplementedException();
    }
}
