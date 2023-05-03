using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance { get; private set; }
    private float timer;

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private State state;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingForEnemyTurn;
    }
    // Update is called once per frame
    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
            return;
        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        if (TakeEnemyAIAction())
                        {
                            state = State.Busy;
                        }
                        else
                        {
                            timer = 0f;
                            TurnSystem.Instance.NextTurn();
                        }
                    }
                }
                break;
            case State.Busy:
                break;
        }    
    }   

    public void OnActionCompleted()
    {
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        state = State.TakingTurn;
        timer = .5f;
    }

    private bool TakeEnemyAIAction()
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TakeEnemyAIAction(enemyUnit))
                return true;
        }
        return false;
    }

    private bool TakeEnemyAIAction(Unit enemyUnit)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if (enemyUnit.GetActionPoints() == 0)
            {
                continue;
            }
            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }
        if (bestEnemyAIAction != null && enemyUnit.GetActionPoints() > 0)
        {
            if (bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition))
                enemyUnit.SubtractActionPoint();
            return true;
        }
        else
        {
            return false;
        }
    }
}
