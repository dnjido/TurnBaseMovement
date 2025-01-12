using System.Collections.Generic;
using UnityEngine;

public interface IGenerateMap
{
    public void GenerateNew();
    public void GenerateLoad();
}

public class TitleGenerate : MonoBehaviour, IGenerateMap  // Генерирует игровую карту
{
    [SerializeField] private GenerateAlgorithm algorithm;
    [SerializeField] private GameObject groundPrefab, bridgePrefab, playerPrefab, parent;
    [SerializeField] public int seed;

    public GameObject player;

    void Awake()
    {
        SpawnPlayer();
    }

    public void Generate()
    {
        Random.InitState(seed);
        algorithm.Generate();
        MakeTiles();
        MakeBridgeX();
        MakeBridgeY();
    }

    public void GenerateNew() // Генерация при новой игре
    {
        Generate();
        MovePlayer();
        MoveCamera();
        GameCheck.Start();
    }

    public void GenerateLoad() // Генерация при загрузке игры
    {
        Generate();
        MoveCamera();
        GameCheck.Start();
    }

     private void MakeTiles() // Создает острова
    {
        foreach (var tile in algorithm.tiles)
        {
            if (tile == null) continue;

            Vector3 pos = new Vector3(tile.position.x - algorithm.halfSize.x, 
                .5f, 
                tile.position.y - algorithm.halfSize.y);

            GameObject ground = MakeGameObject(groundPrefab, pos);
            ground.transform.localScale = new Vector3(tile.size.x, 1, tile.size.y);
        }
     }

     private void MakeBridgeX() // Создает мосты по оси X
    {
         for (int x = 0; x < algorithm.tiles.GetLength(0) - 1; x++)
             for (int y = 0; y < algorithm.tiles.GetLength(1); y++)
             {
                if (algorithm.tiles[x, y] == null) continue;
                if (algorithm.tiles[x + 1, y] == null) continue;

                Tile currentTile = algorithm.tiles[x, y];

                 Vector3 pos = new Vector3(
                     currentTile.position.x + (currentTile.areaSize.x / 2) - algorithm.halfSize.x, 
                     .85f, 
                     currentTile.position.y - algorithm.halfSize.y);

                 GameObject bridge = MakeGameObject(bridgePrefab, pos);
                 bridge.transform.localScale = new Vector3(currentTile.areaSize.x, bridge.transform.localScale.y, bridge.transform.localScale.z);
             }
     }

     private void MakeBridgeY() // Создает мосты по оси Y
    {
        for (int x = 0; x < algorithm.tiles.GetLength(0); x++)
            for (int y = 0; y < algorithm.tiles.GetLength(1) - 1; y++)
            {
                if (algorithm.tiles[x, y] == null) continue;
                if (algorithm.tiles[x, y + 1] == null) continue;

                Tile currentTile = algorithm.tiles[x, y];

                Vector3 pos = new Vector3(
                    currentTile.position.x - algorithm.halfSize.x, 
                    .85f, 
                    currentTile.position.y + (currentTile.areaSize.y / 2) - algorithm.halfSize.y);

                GameObject bridge = MakeGameObject(bridgePrefab, pos);
                bridge.transform.localScale = new Vector3(bridge.transform.localScale.z, bridge.transform.localScale.y, currentTile.areaSize.x);
            }
    }

    private GameObject MakeGameObject(GameObject entity, Vector3 pos) // Создает объекты и делает их дочерними для объекта карты
    {
        GameObject e = Instantiate(entity, pos, entity.transform.rotation);
        e.transform.SetParent(parent.transform);

        return e;
    }

    private void SpawnPlayer() // Создает игрока
    {
        player = Instantiate(playerPrefab, Vector3.zero, playerPrefab.transform.rotation);
    }

    private void MovePlayer() // Перемещает игрока
    {
        List<Tile> list = new List<Tile>();

        foreach (Tile tile in algorithm.tiles)
            if (tile != null) list.Add(tile);

        int randomTile = Random.Range(0, list.Count);

        Vector3 pos = Vector3.zero;
        pos.x = list[randomTile].position.x - algorithm.halfSize.x;
        pos.y = 0 + playerPrefab.transform.position.y;
        pos.z = list[randomTile].position.y - algorithm.halfSize.y;

        player.transform.position = pos;
    }

    private void MoveCamera() // Перемещает камеру
    {
        Vector3 cameraPos = new Vector3(
            player.transform.position.x,
            Camera.main.transform.position.y,
            player.transform.position.z - 15);

        Camera.main.transform.position = cameraPos;
    }
}
