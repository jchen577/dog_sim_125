using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Ensure this is included at the top

public class EnemyAI : MonoBehaviour
{
    public float roamingSpeed = 2f;
    public float chasingSpeed = 4f;
    public float detectRange = 10f;
    public float alertTime = 1.5f; // Time spent in Alerting state
    public float stunDuration = 2f; // Duration of the stun

    public GameObject visualIndicator; // Assign a child object for visual indicator

    private Transform player;
    private Vector3 roamPosition;
    private float roamRadius = 15f;
    private float alertTimer;

    private float stunTimer;
    private enum State
    {
        Roaming,
        Alerting,
        Chasing,
        Stunned
    }
    private State currentState;
    private Animator animator;

    private void Start()
{
    player = GameObject.FindGameObjectWithTag("Player").transform;
    currentState = State.Roaming;
    SetNewRoamingPosition();
    animator = GetComponent<Animator>();

    // Initialize visual indicator
    UpdateVisualIndicator("?");
}

private void Update()
{
    switch (currentState)
    {
        case State.Roaming:
            RoamingBehavior();
            CheckForPlayer();
            if (animator) // Check if the animator exists
            {
                animator.SetBool("roam", true);
                animator.SetBool("chase", false);
                animator.SetBool("stun", false);
            }
            UpdateVisualIndicator("?");
            break;
        case State.Alerting:
            AlertingBehavior();
            UpdateVisualIndicator("!");
            break;
        case State.Chasing:
            ChasingBehavior();
            if (animator) // Check if the animator exists
            {
                animator.SetBool("roam", false);
                animator.SetBool("chase", true);
                animator.SetBool("stun", false);
            }
            UpdateVisualIndicator("!");
            break;
        case State.Stunned:
            StunBehavior();
            if (animator) // Check if the animator exists
            {
                animator.SetBool("roam", false);
                animator.SetBool("chase", false);
                animator.SetBool("stun", true);
            }
            UpdateVisualIndicator("X");
            break;
    }
}


    private void SetNewRoamingPosition()
    {
        roamPosition = transform.position + new Vector3(
            Random.Range(-roamRadius, roamRadius),
            0,
            Random.Range(-roamRadius, roamRadius)
        );
    }

    private void RoamingBehavior()
    {
        transform.position = Vector3.MoveTowards(transform.position, roamPosition, roamingSpeed * Time.deltaTime);
        transform.LookAt(roamPosition);
        if (Vector3.Distance(transform.position, roamPosition) < 0.5f)
        {
            SetNewRoamingPosition();
        }
    }

    private void CheckForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectRange)
        {
            currentState = State.Alerting;
            alertTimer = alertTime;
        }
    }

    private void AlertingBehavior()
    {
        // Stop moving and face the player
        transform.LookAt(player.position);
        alertTimer -= Time.deltaTime;

        if (alertTimer <= 0)
        {
            currentState = State.Chasing;
        }
    }

    private void ChasingBehavior()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chasingSpeed * Time.deltaTime);
        transform.LookAt(player.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectRange)
        {
            currentState = State.Roaming;
            SetNewRoamingPosition();
            UpdateVisualIndicator("?"); // Update visual indicator back to "?" when AI starts roaming
        }
    }
    private void StunBehavior()
    {
        stunTimer -= Time.deltaTime;

        if (stunTimer <= 0)
        {
            // Return to either roaming or chasing based on the player's range
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectRange)
            {
                currentState = State.Chasing;
                UpdateVisualIndicator("!");
            }
            else
            {
                currentState = State.Roaming;
                UpdateVisualIndicator("?");
            }
        }
    }

    public void Stun()
    {
        if (currentState != State.Stunned) // Prevent re-stunning
        {
            currentState = State.Stunned;
            stunTimer = stunDuration;
        }
    }

    private void UpdateVisualIndicator(string symbol)
    {
        // Update the visual indicator's text
        if (visualIndicator != null)
        {
            TextMeshPro textMeshPro = visualIndicator.GetComponent<TextMeshPro>();
            if (textMeshPro != null)
            {
                textMeshPro.text = symbol;
            }
        }
    }
}
