using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : Pathfinder
{
    protected override List<Node> SearchNeighbor(Node current)
    {
        int width = Map.instance.width;
        int height = Map.instance.height;
        List<Node> newNodes = new List<Node>();

        int[] deltaX = {1, 0, -1, 0};
        int[] deltaY = {0, 1, 0, -1};
        for (int i = 0; i < 4; ++i)
        {
            int newX = current.x + deltaX[i];
            int newY = current.y + deltaY[i];
            if (newX < 0 || width <= newX || newY < 0 || height <= newY)
            {
                continue;
            }
            if (Map.instance.IsObstacle(newX, newY))
            {
                continue;
            }

            Node newNode = new Node(newX, newY, current.cost + 10, current);
            if (_visitedNodes.Contains(newNode) == false)
            {
                newNodes.Add(newNode);
            }
        }

        return newNodes;
    }
}
