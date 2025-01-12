using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XMLManager : MonoBehaviour // Работа с XML файлом
{
    private string filePath;

    private void Awake()
    {
        // Устанавливаем путь к файлу
        filePath = Path.Combine(Application.persistentDataPath, "game_data.xml");
    }

    public void SaveData()
    {
        GameData gameData = new GameData();
        gameData.seed = GetComponent<TitleGenerate>().seed;

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in units)
        {
            UnitData data = new UnitData
            {
                name = unit.name,
                x = unit.transform.position.x,
                y = unit.transform.position.y,
                z = unit.transform.position.z,
                turnLeft = unit.GetComponent<CharacterMovement>().turnsLeft
            };
            gameData.units.Add(data);
        }

        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(stream, gameData);
        }
    }

    public void LoadData()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
            GameData gameData = (GameData)serializer.Deserialize(stream);

            foreach (UnitData data in gameData.units)
            {
                GameObject unit = GameObject.Find(data.name);
                if (unit != null)
                {
                    unit.transform.position = new Vector3(data.x, data.y, data.z);
                    unit.GetComponent<CharacterMovement>().SetTurns(data.turnLeft);
                }
            }
            GetComponent<TitleGenerate>().seed = gameData.seed;
        }
    }
}

[System.Serializable]
public class UnitData
{
    public string name;
    public float x;
    public float y;
    public float z;
    public int turnLeft;
}

[System.Serializable]
public class GameData
{
    public List<UnitData> units = new List<UnitData>();
    public int seed;
}