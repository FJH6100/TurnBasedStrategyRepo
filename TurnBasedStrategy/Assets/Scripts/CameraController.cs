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
    private const float MIN_OFFSET = 4f;
    private const float MAX_OFFSET = 14f;
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }
    private void Update()
    {
        if (Input.touchCount > 0)
            Debug.Log("Touched");
        Vector3 inputMoveDirection = new Vector3(0,0,0);
        if (Input.GetAxisRaw("Vertical") > 0)
            inputMoveDirection.z += 1f;
        if (Input.GetAxisRaw("Vertical") < 0)
            inputMoveDirection.z -= 1f;
        if (Input.GetAxisRaw("Horizontal") < 0 && !Input.GetMouseButton(1))
            inputMoveDirection.x -= 1f;
        if (Input.GetAxisRaw("Horizontal") > 0 && !Input.GetMouseButton(1))
            inputMoveDirection.x += 1f;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            if (touchDeltaPosition.y > 1)
                inputMoveDirection.z += 1f;
            if (touchDeltaPosition.y < -1)
                inputMoveDirection.z -= 1f;
            if (touchDeltaPosition.x > 1)
                inputMoveDirection.x -= 1f;
            if (touchDeltaPosition.x < -1)
                inputMoveDirection.x += 1f;
        }

        Vector3 moveVector = inputMoveDirection.z * transform.forward + inputMoveDirection.x * transform.right;
        transform.position += moveVector * cameraSpeed * Time.deltaTime;

        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (Input.GetAxisRaw("Horizontal") < 0 && Input.GetMouseButton(1))
            rotationVector.y -= 1f;
        if (Input.GetAxisRaw("Horizontal") > 0 && Input.GetMouseButton(1))
            rotationVector.y += 1f;

        if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(1).deltaPosition;
            if (touchDeltaPosition.x < 1)
                rotationVector.y -= 1f;
            if (touchDeltaPosition.x > -1)
                rotationVector.y += 1f;
        }

        transform.eulerAngles += rotationVector;

        if (Input.mouseScrollDelta.y > 0f)
            targetFollowOffset.y += 1f;
        if (Input.mouseScrollDelta.y < 0f)
            targetFollowOffset.y -= 1f;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_OFFSET, MAX_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * 5f);
    }
}
