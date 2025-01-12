using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public class Grid
{
    private Vector2Int offset;
    private GameObject unit;

    [SerializeField] private GameObject gridPrefab, parent;

    Vector3 position => unit.transform.position;

    public void Init(Vector2Int _offset, GameObject _unit)
    {
        offset = _offset;
        unit = _unit;
    }

    public void GridMaker() // —оздает клетки, отображающие куда можно перемещатьс€
    {
        parent = new GameObject("Grid");

        for (int x = -offset.y; x < offset.y; x++)
            for (int y = -offset.x; y < offset.x; y++)
            {
                Vector3 pos = new Vector3(x + position.x, 10, y + position.z);

                if (CheckGround.Check(pos))
                {
                    GameObject grid = Object.Instantiate(gridPrefab, CellPos(pos), gridPrefab.transform.rotation);
                    grid.transform.SetParent(parent.transform);
                }
            }
    }

    private Vector3 CellPos(Vector3 vec) // «адает позицию клатки
    {
        Vector3Int pos = Vector3Int.FloorToInt(vec);

        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit, int.MaxValue, 1 << 3))
            return hit.point;

        return Vector3.zero;
    }

    public void ClearGrid() =>
        Object.Destroy(parent);
}

public static class CheckGround // ѕровер€ет поверхность на доступность передвижени€
{
    public static bool Check(Vector3 vec)
    {
        Vector3Int pos = Vector3Int.FloorToInt(vec);

        Vector3 boxSize = new Vector3(1, 20, 1);

        Collider[] colliders = Physics.OverlapBox(pos, boxSize, Quaternion.identity);

        foreach (Collider col in colliders)
        {
            if (col.transform.gameObject.tag == "Obstacle") return false;
            if (col.transform.gameObject.tag == "Ground") return true;
        }
        return false;
    }
}
