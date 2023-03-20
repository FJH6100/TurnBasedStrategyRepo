using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float spinDegrees;
    protected override void Awake()
    {
        base.Awake();
        spinDegrees = 0;
    }
    public override bool TakeAction(GridPosition gridPosition)
    {
        if (IsValidActionGridPosition(gridPosition))
        {
            isActive = true;
            UnitActionSystem.Instance.SetBusy();
            return true;
        }
        else
        {
            return false;
        }
    }

    public override string GetActionName()
    {
        return "SPIN";
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        validGridPositionList.Add(unitGridPosition);
        return validGridPositionList;
    }
    private void Update()
    {
        if (isActive)
        {
            float spinAmount = 720f * Time.deltaTime;
            spinDegrees += spinAmount;
            transform.eulerAngles += new Vector3(0, spinAmount, 0);
            if (spinDegrees >= 360)
            {
                isActive = false;
                spinDegrees = 0;
                UnitActionSystem.Instance.ClearBusy();
                if (GetComponent<Unit>().IsEnemy())
                    EnemyAI.Instance.OnActionCompleted();
            }
        }
    }
}
