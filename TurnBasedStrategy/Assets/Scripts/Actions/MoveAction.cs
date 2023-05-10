using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    private List<Vector3> positionList;
    [SerializeField]
    private Animator unitAnimator;
    [SerializeField]
    private int maxMoveDistance = 4;
    private int currentPositionIndex;

    private void Update()
    {
        if (!isActive)
            return;
        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float stoppingDistance = .1f;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
        {           
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;            
            unitAnimator.SetBool("Moving", true);
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                unitAnimator.SetBool("Moving", false);
                isActive = false;
                UnitActionSystem.Instance.ClearBusy();
                if (GetComponent<Unit>().IsEnemy())
                    EnemyAI.Instance.OnActionCompleted();
            }
        }
    }

    public override bool TakeAction(GridPosition gridPosition)
    {
        if (IsValidActionGridPosition(gridPosition))
        {
            List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);
            currentPositionIndex = 0;
            positionList = new List<Vector3>();
            foreach (GridPosition g in gridPositionList)
            {
                positionList.Add(LevelGrid.Instance.GetWorldPosition(g));
            }
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
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                    continue;
                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                    continue;
                //Path length too long
                int pathfindingDistanceMultiplier = 10;
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
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
        int targetCount = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCount * 10,
            //actionValue = 0,
        };
    }
}
