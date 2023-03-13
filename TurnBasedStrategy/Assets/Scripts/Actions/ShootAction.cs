using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    [SerializeField]
    private int maxShootDistance = 4;
    [SerializeField]
    private int shootDamage = 50;
    private Transform target;
    public override string GetActionName()
    {
        return "SHOOT";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        int oneMoreMax = maxShootDistance + 1;
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -oneMoreMax; x <= oneMoreMax; x++)
        {
            for (int z = -oneMoreMax; z <= oneMoreMax; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                int gridPositionValue = unitGridPosition.x + unitGridPosition.z;
                int testPositionValue = testGridPosition.x + testGridPosition.z;
                int gridPositionValueNeg = unitGridPosition.x - unitGridPosition.z;
                int testPositionValueNeg = testGridPosition.x - testGridPosition.z;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                //Radial check
                if (Mathf.Abs(gridPositionValue - testPositionValue) > oneMoreMax || Mathf.Abs(gridPositionValueNeg - testPositionValueNeg) > oneMoreMax)
                    continue;
                //Round the circle
                if (Mathf.Abs(testGridPosition.x - unitGridPosition.x) == oneMoreMax || Mathf.Abs(testGridPosition.z - unitGridPosition.z) == oneMoreMax)
                    continue;
                if (!LevelGrid.Instance.UnitOnGridPosition(testGridPosition))
                    continue;
                if (LevelGrid.Instance.GetUnitListAtGridPosition(testGridPosition)[0].IsEnemy())
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override bool TakeAction(Vector3 position)
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(position);
        if (IsValidActionGridPosition(gridPosition))
        {
            target = LevelGrid.Instance.GetUnitListAtGridPosition(gridPosition)[0].transform;
            isActive = true;
            UnitActionSystem.Instance.SetBusy();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        if (isActive)
        {
            Vector3 moveDirection = (target.position - transform.position).normalized;
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
            if (Vector3.Dot(transform.forward.normalized, moveDirection.normalized) > .99f)
            {
                isActive = false;
                if (target.GetComponent<Unit>() != null)
                    target.GetComponent<Unit>().TakeDamage(shootDamage);
                target = null;
                UnitActionSystem.Instance.ClearBusy();
            }
        }
    }
}
