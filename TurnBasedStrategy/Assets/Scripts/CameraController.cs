using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 10f;
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private const float MIN_OFFSET = 2f;
    private const float MAX_OFFSET = 12f;
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }
    private void Update()
    {
        Vector3 inputMoveDirection = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.W))
            inputMoveDirection.z += 1f;
        if (Input.GetKey(KeyCode.S))
            inputMoveDirection.z -= 1f;
        if (Input.GetKey(KeyCode.A) && !Input.GetMouseButton(1))
            inputMoveDirection.x -= 1f;
        if (Input.GetKey(KeyCode.D) && !Input.GetMouseButton(1))
            inputMoveDirection.x += 1f;

        Vector3 moveVector = inputMoveDirection.z * transform.forward + inputMoveDirection.x * transform.right;
        transform.position += moveVector * cameraSpeed * Time.deltaTime;

        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.D) && Input.GetMouseButton(1))
            rotationVector.y -= 1f;
        if (Input.GetKey(KeyCode.A) && Input.GetMouseButton(1))
            rotationVector.y += 1f;
        transform.eulerAngles += rotationVector;

        if (Input.mouseScrollDelta.y > 0f)
            targetFollowOffset.y += 1f;
        if (Input.mouseScrollDelta.y < 0f)
            targetFollowOffset.y -= 1f;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_OFFSET, MAX_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * 5f);
    }
}
