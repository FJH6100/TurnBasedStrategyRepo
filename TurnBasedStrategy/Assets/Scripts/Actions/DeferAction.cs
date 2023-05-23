using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeferAction : BaseAction
{
    [SerializeField]
    private int deferDistance = 1;
    [SerializeField]
    private Animator unitAnimator;
    [SerializeField]
    private Transform bulletProjectilePrefab;
    [SerializeField]
    private Transform shootPointPrefab;
    [SerializeField]
    private LayerMask obstaclesLayerMask;
    private Transform target;
    public override string GetActionName()
    {
        return "DEFER";
    }

    public override bool TakeAction(GridPosition gridPosition)
    {
        if (IsValidActionGridPosition(gridPosition))
        {
            target = LevelGrid.Instance.GetUnitListAtGridPosition(gridPosition)[0].transform;
            isActive = true;
            UnitActionSystem.Instance.SetBusy();
            if (target.GetComponent<Unit>() != null)
            {
                int deferPoints = unit.GetActionPoints();
                unit.SetActionPoints(0);
                target.GetComponent<Unit>().SetActionPoints(target.GetComponent<Unit>().GetActionPoints() + deferPoints);
            }
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
                StartCoroutine(ShootAnim());
                target = null;
                UnitActionSystem.Instance.ClearBusy();
                if (unit.IsEnemy())
                    EnemyAI.Instance.OnActionCompleted();
            }
        }
    }

    IEnumerator ShootAnim()
    {
        unitAnimator.SetTrigger("Shoot");
        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointPrefab.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();
        Vector3 targetPosition = target.position;
        targetPosition.y = shootPointPrefab.position.y;
        bulletProjectile.Setup(targetPosition);
        yield return new WaitForSeconds(.5f);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        int oneMoreMax = deferDistance + 1;
        List<GridPosition> validGridPositionList = new List<GridPosition>();
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
                if (testGridPosition == unit.GetGridPosition())
                    continue;
                Unit targetUnit = LevelGrid.Instance.GetUnitListAtGridPosition(testGridPosition)[0];
                if (targetUnit.IsEnemy() != unit.IsEnemy())
                {
                    //If units are on different teams
                    continue;
                }
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float unitShoulderHeight = 1.7f;
                //Check if blocked by obstacle
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight,
                    shootDirection,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    obstaclesLayerMask))
                    continue;
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
}
