using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code modified from https://youtu.be/rJqP5EesxLk?feature=shared

public class PlayerMovement: MonoBehaviour {
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 jumpForward;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;
    public float jumpForwardBoost = 2f;
    public float pushForce = 50f;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        bool wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        if (isGrounded && !wasGrounded) {
            jumpForward = Vector3.zero;
        }
    }

    // Receive the inputs for our InputManager.cs and apply them to our character controller
    public void ProcessMove(Vector2 input) {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        // apply gravity
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0) {
            playerVelocity.y = -2f;
        }

        controller.Move(jumpForward * Time.deltaTime);

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump() {
        if (isGrounded) {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);

            // Apply a forward leap boost
            jumpForward = transform.forward * jumpForwardBoost;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody rb = hit.collider.attachedRigidbody;

        // Apply a force if the collided object has a Rigidbody and it’s not kinematic
        if (rb != null && !rb.isKinematic) {
            rb.AddForce(hit.moveDirection * pushForce);
        }
    }
}
