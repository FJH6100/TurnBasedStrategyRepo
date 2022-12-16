using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField]
    private Animator unitAnimator;
    [SerializeField]
    private int maxMoveDistance = 4;
    private Unit unit;

    private void Start()
    {
        targetPosition = transform.position;
        unit = GetComponent<Unit>();
    }

    private void Update()
    {
        float stoppingDistance = .1f;
        if (Vector3.Distance(targetPosition, transform.position) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
            unitAnimator.SetBool("Moving", true);
        }
        else
        {
            unitAnimator.SetBool("Moving", false);

        }
    }

    public void Move(Vector3 MovePosition)
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(MovePosition);
        if (IsValidActionGridPosition(gridPosition))
            targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
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
                if (Mathf.Abs(gridPositionValue - testPositionValue) > maxMoveDistance || Mathf.Abs(gridPositionValueNeg - testPositionValueNeg) > maxMoveDistance)
                    continue;
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
}
