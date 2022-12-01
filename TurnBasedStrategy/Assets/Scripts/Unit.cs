using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 TargetPosition;

    private void Update()
    {
        Vector3 MoveDirection = (TargetPosition - transform.position).normalized;
        float MoveSpeed = 4f;
        float StoppingDistance = .1f;
        if (Vector3.Distance(TargetPosition, transform.position) > StoppingDistance)
            transform.position += MoveDirection * MoveSpeed * Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }
    private void Move(Vector3 MovePosition)
    {
        TargetPosition = MovePosition;
    }    
}
