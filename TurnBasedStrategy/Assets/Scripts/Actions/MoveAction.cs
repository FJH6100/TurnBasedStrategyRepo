using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private Vector3 targetPosition;
    [SerializeField]
    private Animator unitAnimator;
    [SerializeField]
    private int maxMoveDistance = 4;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive)
            return;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float stoppingDistance = .1f;
        if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
        {           
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;            
            unitAnimator.SetBool("Moving", true);
        }
        else
        {
            unitAnimator.SetBool("Moving", false);
            isActive = false;
            UnitActionSystem.Instance.ClearBusy();
            if (GetComponent<Unit>().IsEnemy())
                EnemyAI.Instance.OnActionCompleted();
        }
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
    }

    public override bool TakeAction(GridPosition gridPosition)
    {
        if (IsValidActionGridPosition(gridPosition))
        {
            targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
            isActive = true;
            UnitActionSystem.Instance.SetBusy();
            return true;
        }
        else
        {
            return false;
        }
    }

    //Search for one larger than max move range, then create the circle
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        int oneMoreMax = maxMoveDistance + 1;
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
                //If same position
                if (unitGridPosition == testGridPosition)
                    continue;
                if (LevelGrid.Instance.UnitOnGridPosition(testGridPosition))
                    continue;
                //Radial check
                if (Mathf.Abs(gridPositionValue - testPositionValue) > oneMoreMax || Mathf.Abs(gridPositionValueNeg - testPositionValueNeg) > oneMoreMax)
                    continue;
                //Round the circle
                if (Mathf.Abs(testGridPosition.x - unitGridPosition.x) == oneMoreMax || Mathf.Abs(testGridPosition.z - unitGridPosition.z) == oneMoreMax)
                    continue;
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "MOVE";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCount = unit.GetShootAction().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCount * 10,
            //actionValue = 0,
        };
    }
}
