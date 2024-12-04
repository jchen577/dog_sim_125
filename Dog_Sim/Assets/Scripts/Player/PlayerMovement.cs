using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 jumpForward;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;
    public float jumpForwardBoost = 2f;
    public float pushForce = 50f;

    public float biteForce = 5f; // Strength of the bite push
    public float biteRange = 2f; // Range of the bite attack

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        if (isGrounded && !wasGrounded)
        {
            jumpForward = Vector3.zero;
        }

        // Check for bite input (using space as an example)
        if (Input.GetMouseButtonDown(0)) // Replace with your preferred input
        {
            Bite();
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        // apply gravity
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        controller.Move(jumpForward * Time.deltaTime);

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);

            // Apply a forward leap boost
            jumpForward = transform.forward * jumpForwardBoost;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        // Apply a force if the collided object has a Rigidbody and it’s not kinematic
        if (rb != null && !rb.isKinematic)
        {
            rb.AddForce(hit.moveDirection * pushForce);
        }
    }

    // Implement the bite mechanic
    private void Bite()
    {
        // Find nearby objects within the bite range
        Collider[] colliders = Physics.OverlapSphere(transform.position, biteRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy")) // Check if the collider belongs to an AI enemy
            {
                EnemyAI enemyAI = collider.GetComponent<EnemyAI>();

                if (enemyAI != null)
                {
                    enemyAI.Stun(); // Trigger the stun state
                }

                Rigidbody enemyRb = collider.GetComponent<Rigidbody>();

                if (enemyRb != null)
                {
                    // Calculate pushback direction (away from the player)
                    Vector3 pushDirection = (collider.transform.position - transform.position).normalized;

                    // Apply force to push the enemy away
                    enemyRb.AddForce(pushDirection * biteForce, ForceMode.Impulse);
                }
            }
        }
    }

}