using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private BaseAction[] baseActionArray;
    [SerializeField]
    private int actionPoints = 2;
    [SerializeField]
    private bool isEnemy;
    private int defaultActionPoints;

    private void Awake()
    {
        //Find the grid position closest to where the character is placed and move the character there.
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        transform.position = LevelGrid.Instance.GetWorldPosition(gridPosition);
        
        this.name = "Unit";
        baseActionArray = GetComponents<BaseAction>();
        defaultActionPoints = actionPoints;
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public void SubtractActionPoint()
    {
        if (actionPoints > 0)
            actionPoints--;
    }

    public void RestoreActionPoints()
    {
        actionPoints = defaultActionPoints;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }
       
}
