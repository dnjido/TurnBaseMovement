using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EndTurn : MonoBehaviour // Восстанавливает очки передвижения
{
    private CharacterMovement movement;

    private void Start()
    {
        movement = GameObject.FindWithTag("Unit").GetComponent<CharacterMovement>();
    }

    public void End() =>
        movement.RecoveryTurns();
}
