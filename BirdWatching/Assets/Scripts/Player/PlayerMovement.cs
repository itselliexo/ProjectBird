using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    [SerializeField] private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;


    private void Update()
    {
        HandleMovement();
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

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        characterController.Move(move * speed * Time.deltaTime);

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        velocity.y -= gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}
