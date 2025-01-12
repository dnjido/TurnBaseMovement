using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour // ��������� � ��������� ����
{
    private XMLManager XMLManager => GetComponent<XMLManager>();
    private IGenerateMap generateMap => GetComponent<IGenerateMap>();

    // �������������� ������ ��� ������
    void Start() =>
        TryLoad();

    // ���������� ������ ��� ������
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