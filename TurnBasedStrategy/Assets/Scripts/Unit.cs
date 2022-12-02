using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Animator UnitAnimator;
    private Vector3 TargetPosition;

    private void Awake()
    {
        TargetPosition = transform.position;
    }
    private void Update()
    {
        float StoppingDistance = .1f;
        if (Vector3.Distance(TargetPosition, transform.position) > StoppingDistance)
        {
            Vector3 MoveDirection = (TargetPosition - transform.position).normalized;
            float MoveSpeed = 4f;
            transform.position += MoveDirection * MoveSpeed * Time.deltaTime;

            float RotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, MoveDirection, RotateSpeed * Time.deltaTime);
            UnitAnimator.SetBool("Moving", true);
        }
        else
        {
            UnitAnimator.SetBool("Moving", false);
        }
    }
    public void Move(Vector3 MovePosition)
    {
        TargetPosition = MovePosition;
    }    
}
