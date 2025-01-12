using UnityEngine;

public class Node // ����� �� ������� �������� ����� ��� ���������
{
    public Vector2Int Position; // ������� ���� �� �����
    public float GCost; // ��������� ����������� �� ���������� ����
    public float HCost; // ������������� ��������� �� �������� ����
    public float FCost => GCost + HCost; // ����� ���������
    public Node Parent; // ����-��������

    public Node(Vector2Int position)
    {
        Position = position;
    }
}