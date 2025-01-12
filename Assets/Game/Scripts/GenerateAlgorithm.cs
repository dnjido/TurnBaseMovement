using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerateAlgorithm // Алгоритм генерации островов
{
    [SerializeField] private Vector2Int size, count, offset;
    [SerializeField] [Range(0, 100)] private float chance = 80f;

    public Tile[,] tiles;

    public Vector2Int halfSize => size / 2;

    public void Generate() // Разделение карты на зоны
    {
        tiles = new Tile[count.x, count.y];

        Vector2Int areasSize = new Vector2Int(size.x / count.x, size.y / count.y);

        for (int x = 0; x < count.x; x++)
            for (int y = 0; y < count.y; y++)
            {
                float random = Random.Range(0, 100);
                if (random > chance) continue;

                Vector2Int center = Vector2Int.zero;

                center.x = areasSize.x * x + (areasSize.x / 2);
                center.y = areasSize.y * y + (areasSize.y / 2);
                tiles[x, y] = new Tile(center, areasSize, new Vector2Int(x, y));
            }
    }
}

[System.Serializable]
public class Tile  // Алгоритм генерации острова
{
    public readonly Vector2Int ID;
    public readonly Vector2Int position;
    public readonly Vector2Int areaSize;
    public Vector2Int size;

    public Vector2Int center => areaSize / 2;

    public Tile(Vector2Int _position, Vector2Int _size, Vector2Int id)  // Задает размер острова
    {
        ID = id;
        position = _position;
        areaSize = _size;
        SetSize();
    }

    public void SetSize()
    {
        size.x = Random.Range(areaSize.x / 2, areaSize.x - 2);
        size.y = Random.Range(areaSize.y / 2, areaSize.y - 2);

        size.x = size.x / 2 * 2 + 1;
        size.y = size.y / 2 * 2 + 1;
    }
}
