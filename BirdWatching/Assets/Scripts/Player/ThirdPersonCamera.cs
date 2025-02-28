using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] Vector3 boom;

    [Header("Mouse Settings")]
    [SerializeField] float sensitivity;
    [SerializeField] float rotationSmoothTime;
    [SerializeField] float lowerRotLimit;
    [SerializeField] float upperRotLimit;

    private float yaw;
    private float pitch;
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    private void LateUpdate()
    {
        if (!player) return;

        HandleRotation();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HandleRotation()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, lowerRotLimit, upperRotLimit);

        Vector3 targetRotation = new Vector3(pitch, yaw, 0);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        Quaternion rotation = Quaternion.Euler(currentRotation);

        Vector3 desiredPosition = player.transform.position + rotation * boom;

        mainCamera.transform.position = desiredPosition;

       /*RaycastHit hit;
        if (Physics.Raycast(player.transform.position, (desiredPosition - player.transform.position).normalized, out hit, boom.magnitude))
        {
            mainCamera.transform.position = hit.point + hit.normal * 0.3f;
        }
        else
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, Time.deltaTime * 10);
        }*/

            mainCamera.transform.LookAt(player.transform.position + Vector3.up * boom.y);
    }
}
