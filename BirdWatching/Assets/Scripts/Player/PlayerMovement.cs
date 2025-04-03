using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float currentSpeed;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float defaultSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    private Vector3 velocity;
    private bool isGrounded;
    public bool canMove = true;


    private void Update()
    {
        if (canMove)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? maxSpeed: defaultSpeed;

        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * moveX + camForward * moveZ;
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        characterController.Move(move * currentSpeed * Time.deltaTime);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        velocity.y -= gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}
