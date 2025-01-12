using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour // Передвижение персонажа
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int turnsCount;
    
    private Vector3 oldPosition;
    [SerializeField] public int turnsLeft;

    [SerializeField] private Grid grid;

    private Vector2Int startPosition, targetPosition;
    private AStar aStar;
    private List<Node> path;
    private int currentPathIndex;
    private bool moved;

    private Vector3 movePoint;

    private Vector2Int halfSize => gridSize / 2;

    public delegate void TurnDelegate(int count);
    public event TurnDelegate TurnEvent;

    private void Awake()
    {
        RecoveryTurns();

        grid.Init(halfSize, gameObject);
        GameCheck.game.StartEvent += Init;
    }

    private void Init() =>
        StartCoroutine(InitCoroutine());

    IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        grid.GridMaker();
        GameCheck.game.StartEvent -= Init;
    }

    private void FindPath()  // Поиск пути от начала к концу
    {
        if (turnsLeft == 0 || moved || !CheckGround.Check(CursorPosition.RayHit())) return;

        oldPosition = transform.position;

        aStar = new AStar(gridSize, NodesMaker()); // Размер сетки

        targetPosition = InverseTransform(CursorPosition.RayHit());
        startPosition = InverseTransform(transform.position);

        path = aStar.FindPath(startPosition + halfSize, targetPosition + halfSize);

        if (path.Count == 0) return;

        grid.ClearGrid();
        currentPathIndex = 0;
        moved = true;
    }

    private Vector2Int InverseTransform(Vector3 vec) // Преобразовывает координаты сетки в координаты сцены
    {
        Vector3 localPosition = transform.InverseTransformPoint(vec);
        return CursorPosition.ConvertTo2dInt(localPosition);
    }

    private void TryPath()
    {
        try { FindPath(); }
        catch { Debug.LogWarning("Путь не найден"); }
    }

    void Update()
    {
        CheckGround.Check(CursorPosition.RayHit());
        if (Input.GetMouseButtonDown(0)) TryPath();

        if (path != null && moved && turnsLeft > 0) 
            MoveAlongPath();
    }

    public void RecoveryTurns() // Восстанавливает очки передвижения
    {
        turnsLeft = turnsCount;
        TurnEvent?.Invoke(turnsLeft);
    }

    public void SetTurns(int count) // Изменяет очки передвижения
    {
        turnsLeft = count;
        TurnEvent?.Invoke(turnsLeft);
    }

    private void MoveAlongPath() // Перемещает персонажа к следующему ноду
    {
        Vector3 end = transform.position;

        end.x = path[currentPathIndex].Position.x + oldPosition.x - halfSize.x;
        end.z = path[currentPathIndex].Position.y + oldPosition.z - halfSize.y;

        transform.position = Vector3.MoveTowards(transform.position, end, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, end) < 0.1f)
        {
            turnsLeft--;
            TurnEvent?.Invoke(turnsLeft);

            currentPathIndex++;
            movePoint = end;
            if (turnsLeft <= 0) StopMove();
        }

        if (currentPathIndex >= path.Count)
            StopMove();
    }

    private void StopMove() // Останавливает перемещение
    {
        transform.position = movePoint;

        targetPosition = InverseTransform(oldPosition);
        currentPathIndex = 0;
        moved = false;
        grid.GridMaker();
    }

    private List<Vector2Int> NodesMaker() // Создает ноды
    {
        List<Vector2Int> nodes = new List<Vector2Int>();
        for (int x = -halfSize.y; x < halfSize.y; x++)
            for (int y = -halfSize.x; y < halfSize.x; y++)
            {
                Vector3 pos = new Vector3(x + transform.position.x, 10, y + transform.position.z);

                if (CheckGround.Check(pos)) 
                    nodes.Add(new Vector2Int(x + halfSize.x, y + halfSize.y));
            }
        return nodes;
    }
}
