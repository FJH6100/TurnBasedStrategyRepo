using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    //private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    private bool isMine;

    public GridObject (GridSystem gridSystem, GridPosition gridPosition, bool mine)
    {
        //this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
        isMine = mine;
    }

    public bool GetIsMine()
    {
        return isMine;
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
        if (isMine)
            unit.TakeDamage(25);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
            unitString += unit.name + "\n";
        return "x: " + gridPosition.x + " z: " + gridPosition.z + "\n" + unitString;
    }
}
