using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    [SerializeField] private Transform gridDebugGameObject;

    private GridSystem gridSystem;
    private void Awake()
    {
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugGameObject);
        Instance = this;
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition);
    }   
    
    public void UnitMovedGridPosition(Unit unit, GridPosition from, GridPosition to)
    {
        Debug.Log("Move");
        RemoveUnitAtGridPosition(from, unit);
        AddUnitAtGridPosition(to, unit);
    }

    public bool OutOfRange()
    {
        GridPosition gridPosition = gridSystem.GetGridPosition(MouseWorld.GetPosition());
        if (gridPosition.x < 0 || gridPosition.z < 0 || gridPosition.x >= gridSystem.GetWidth() || gridPosition.x >= gridSystem.GetHeight())
            return true;
        return false;
    }

    public Vector3 GetUnitWorldPosition(Unit unit, GridPosition gridPosition)
    {
        return gridSystem.GetWorldPosition(gridPosition);
    }

    private void Update()
    {
        //Debug.Log(gridSystem.GetGridPosition(MouseWorld.GetPosition()));
    }
}
