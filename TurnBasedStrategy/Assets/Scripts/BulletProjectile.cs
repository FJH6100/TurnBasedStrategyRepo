using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField]
    private float moveSpeed = 200f;
    [SerializeField]
    private TrailRenderer trailRenderer;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(targetPosition, transform.position);
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float distanceAfterMoving = Vector3.Distance(targetPosition, transform.position);
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
        }
    }
}
