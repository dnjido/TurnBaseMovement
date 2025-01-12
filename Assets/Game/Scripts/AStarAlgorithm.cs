using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class AStar // Алгоритм A*, который задает оптимальный от первого нода к последнему
{
    private readonly Node[,] grid;
    private readonly List<Node> openList;
    private readonly HashSet<Node> closedList;

    public AStar(Vector2Int size)
    {
        grid = new Node[size.x, size.y];

        for (int x = 0; x < size.y; x++)
            for (int y = 0; y < size.x; y++)
                grid[x, y] = new Node(new Vector2Int(x, y));

        openList = new List<Node>();
        closedList = new HashSet<Node>();
    }

    public AStar(Vector2Int size, List<Vector2Int> nodes)
    {
        grid = new Node[size.x, size.y];

        foreach (Vector2Int node in nodes) 
            grid[node.x, node.y] = new Node(new Vector2Int(node.x, node.y));

        openList = new List<Node>();
        closedList = new HashSet<Node>();
    }

    public List<Node> FindPath(Vector2Int start, Vector2Int target) // Поиск оптимального пути
    {
        Node startNode = grid[start.x, start.y];
        Node targetNode = grid[target.x, target.y];

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
                if (GetNodeCost(i, currentNode)) currentNode = openList[i];

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.Position == targetNode.Position)
                return RetracePath(startNode, currentNode);

            GetNeighborsNode(currentNode, targetNode);
        }

        return null; // Путь не найден
    }

    private bool GetNodeCost(int i, Node currentNode) =>
        openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost);

    private void GetNeighborsNode(Node currentNode, Node targetNode) // Ищет соседний нод с лучшей стоимостью перемещения
    {
        foreach (Node neighbor in GetNeighbors(currentNode))
        {
            if (closedList.Contains(neighbor)) continue;

            float newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
            if (newCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor))
            {
                neighbor.GCost = newCostToNeighbor;
                neighbor.HCost = GetDistance(neighbor, targetNode);
                neighbor.Parent = currentNode;

                if (!openList.Contains(neighbor)) 
                    openList.Add(neighbor);
            }
        }
    }

    private List<Node> RetracePath(Node startNode, Node endNode) // Задает стартовый нод
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbors(Node node) // Ищет соседние ноды
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.Position.x + x;
                int checkY = node.Position.y + y;

                bool hasNeighbor = checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1) && grid[checkX, checkY] != null;

                if (hasNeighbor) 
                    neighbors.Add(grid[checkX, checkY]);
            }

        return neighbors;
    }

    private float GetDistance(Node a, Node b) // Высчитывает расстояние от текущего нода до соседнего
    {
        try
        {
            int dstX = Mathf.Abs(a.Position.x - b.Position.x);
            int dstY = Mathf.Abs(a.Position.y - b.Position.y);
            return dstX + dstY; // Манхэттенское расстояние
        }
        catch { return 0; }
    }
}