using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour // Сохраняет и загружает игру
{
    private XMLManager XMLManager => GetComponent<XMLManager>();
    private IGenerateMap generateMap => GetComponent<IGenerateMap>();

    // Восстановление данных при старте
    void Start() =>
        TryLoad();

    // Сохранение данных при выходе
    void OnApplicationQuit() =>
        XMLManager.SaveData();

    private void TryLoad()
    {
        try 
        { 
            XMLManager.LoadData();
            generateMap.GenerateLoad();
        } 
        catch
        {
            generateMap.GenerateNew();
        }
    }
}