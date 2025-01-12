using System;
using UnityEngine;

public static class CursorPosition // Определяет положение курсора по отношению к игровому миру
{
    public static Vector3 RayHit()
    {
        RaycastHit hitInfo;
        Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(rayOrigin, out hitInfo, Mathf.Infinity);
        return hitInfo.point;
    }

    public static Vector3Int ConvertTo3dInt(Vector3 pos) =>
        new Vector3Int((int)Math.Round(pos.x), (int)Math.Round(pos.y), (int)Math.Round(pos.z));

    public static Vector2Int ConvertTo2dInt(Vector3 pos) =>
        new Vector2Int((int)Math.Round(pos.x), (int)Math.Round(pos.z));
}
