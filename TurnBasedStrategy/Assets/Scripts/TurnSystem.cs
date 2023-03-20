using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
