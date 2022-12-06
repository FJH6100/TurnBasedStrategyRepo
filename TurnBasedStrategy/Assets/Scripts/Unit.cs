using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Animator unitAnimator;
    private Vector3 targetPosition;
    private GridPosition gridPosition;

    private void Awake()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        transform.position = LevelGrid.Instance.GetUnitWorldPosition(this, gridPosition);
        targetPosition = transform.position;
        this.name = "Unit";
    }
    private void Start()
    {
        
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
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }
    public void Move(Vector3 MovePosition)
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(MovePosition);
        targetPosition = LevelGrid.Instance.GetUnitWorldPosition(this, gridPosition);
    }    
}
