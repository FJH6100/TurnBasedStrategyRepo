using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    private int turnNumber = 1;
    private bool isPlayerTurn = true;
    [SerializeField]
    private GameObject enemyUI;

    private void Awake()
    {
        Instance = this;
    }

    public void NextTurn()
    {
        if (!isPlayerTurn)
            turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        UnitActionSystem.Instance.ActionPointRefresh();
        enemyUI.SetActive(!enemyUI.activeSelf);
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
