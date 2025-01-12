using UnityEngine;

public class Node // Точки из которых строится сетка для навигации
{
    public Vector2Int Position; // Позиция узла на сетке
    public float GCost; // Стоимость перемещения от стартового узла
    public float HCost; // Эвристическая стоимость до целевого узла
    public float FCost => GCost + HCost; // Общая стоимость
    public Node Parent; // Узел-родитель

    public Node(Vector2Int position)
    {
        Position = position;
    }
}