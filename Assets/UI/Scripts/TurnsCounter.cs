using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TurnsCounter : MonoBehaviour // Отображает очки передвижения
{
    private CharacterMovement movement;

    private void Start()
    {
        OnEnable();
    }

    void OnEnable()
    {
        if (!GameObject.FindWithTag("Unit")) return;

        movement = GameObject.FindWithTag("Unit").GetComponent<CharacterMovement>();
        movement.TurnEvent += SetCounter;
    } 

    void OnDisable() =>
        movement.TurnEvent -= SetCounter;

    void SetCounter(int count) =>
        GetComponent<TMP_Text>().text = count.ToString();
}
