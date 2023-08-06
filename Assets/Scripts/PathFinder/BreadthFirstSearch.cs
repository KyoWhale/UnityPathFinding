using System.Collections.Generic;

public class BreadthFirstSearch : Pathfinder
{
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
            
            int cost = i < 4 ? STRAIGHT_COST : DIAGONAL_COST;
            Node newNode = new Node(newX, newY, current.cost + cost, current);
            if (_closedNodes.Contains(newNode))
            {
                continue;
            }
            newNodes.Add(newNode);
        }

        return newNodes;
    }
}
