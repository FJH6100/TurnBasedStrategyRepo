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
    [SerializeField]
    private int healthPoints = 100;
    private int maxHealthPoints;
    public event EventHandler OnDamaged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    private void Awake()
    {
        //Find the grid position closest to where the character is placed and move the character there.
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        transform.position = LevelGrid.Instance.GetWorldPosition(gridPosition);
        baseActionArray = GetComponents<BaseAction>();
        defaultActionPoints = actionPoints;
        maxHealthPoints = healthPoints;
    }

    private void Start()
    {
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
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

    public void TakeDamage(int shootDamage)
    {
        healthPoints -= shootDamage;
        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (healthPoints <= 0)
        {
            LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
            OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
            Destroy(this.gameObject);
        }
    }

    public float ReturnNormalizedHealth()
    {
        return (float)healthPoints / maxHealthPoints;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public SpinAction GetSpinAction()
    {
        return GetComponent<SpinAction>();
    }

    public ShootAction GetShootAction()
    {
        return GetComponent<ShootAction>();
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
