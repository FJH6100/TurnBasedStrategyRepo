using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    [SerializeField]
    private int maxShootDistance = 4;
    [SerializeField]
    private int shootDamage = 50;
    [SerializeField]
    private Animator unitAnimator;
    [SerializeField]
    private Transform bulletProjectilePrefab;
    [SerializeField]
    private Transform shootPointPrefab;
    private Transform target;
    [SerializeField]
    private LayerMask obstaclesLayerMask;
    public override string GetActionName()
    {
        return "SHOOT";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        int oneMoreMax = maxShootDistance + 1;
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
                Unit targetUnit = LevelGrid.Instance.GetUnitListAtGridPosition(testGridPosition)[0];
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //If units are on the same team
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

    public override bool TakeAction(GridPosition gridPosition)
    {
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
                StartCoroutine(ShootAnim());
                if (target.GetComponent<Unit>() != null)
                    target.GetComponent<Unit>().TakeDamage(shootDamage);
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
        if (IsValidActionGridPosition(gridPosition))
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 50,
            };
        }
        else
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = -10,
            };
        }
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
