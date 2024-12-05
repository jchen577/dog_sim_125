using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI; // Import for NavMeshAgent
public class EnemyAI : MonoBehaviour
{
    public Transform initialTarget;
    private bool reachedInitialTarget = false;
    public float roamingSpeed = 2f;
    public float chasingSpeed = 4f;
    public float detectRange = 10f;
    public float alertTime = 1.5f; 
    public float stunDuration = 2f;

    public GameObject visualIndicator;

    public Vector3 storeCenter;
    public Vector3 storeSize;

    private Transform player;
    private Vector3 roamPosition;
    private float alertTimer;
    private float stunTimer;

    private NavMeshAgent navMeshAgent; // NavMeshAgent reference

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
        navMeshAgent = GetComponent<NavMeshAgent>(); // Initialize NavMeshAgent
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent not found on " + gameObject.name);
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = State.Roaming;
        SetNewRoamingPosition();
        animator = GetComponent<Animator>();

        // Set roaming speed in NavMeshAgent
        navMeshAgent.speed = roamingSpeed;

        UpdateVisualIndicator("?");
    }

    private void Update()
    {
        if (!reachedInitialTarget && initialTarget != null)
        {
            MoveToInitialTarget();
        }
        else
        {
            switch (currentState)
            {
                case State.Roaming:
                    RoamingBehavior();
                    CheckForPlayer();
                    if (animator)
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
                    if (animator)
                    {
                        animator.SetBool("roam", false);
                        animator.SetBool("chase", true);
                        animator.SetBool("stun", false);
                    }
                    UpdateVisualIndicator("!");
                    break;
                case State.Stunned:
                    StunBehavior();
                    if (animator)
                    {
                        animator.SetBool("roam", false);
                        animator.SetBool("chase", false);
                        animator.SetBool("stun", true);
                    }
                    UpdateVisualIndicator("X");
                    break;
            }
        }
    }

    private void SetNewRoamingPosition()
    {
        // Generate a random position within the store boundaries
        float randomX = Random.Range(storeCenter.x - storeSize.x / 2, storeCenter.x + storeSize.x / 2);
        float randomZ = Random.Range(storeCenter.z - storeSize.z / 2, storeCenter.z + storeSize.z / 2);
        roamPosition = new Vector3(randomX, transform.position.y, randomZ);

        // Set destination for NavMeshAgent
        navMeshAgent.SetDestination(roamPosition);
    }

    private void RoamingBehavior()
    {
        if (navMeshAgent.remainingDistance < 0.5f) // Check if agent has reached roam position
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

            // Stop the NavMeshAgent temporarily during alert
            navMeshAgent.isStopped = true;
        }
    }

    private void AlertingBehavior()
    {
        transform.LookAt(player.position); // Look at player for visual effect
        alertTimer -= Time.deltaTime;

        if (alertTimer <= 0)
        {
            currentState = State.Chasing;
            navMeshAgent.isStopped = false; // Resume movement
            navMeshAgent.speed = chasingSpeed; // Update speed for chasing
        }
    }

    private void ChasingBehavior()
    {
        navMeshAgent.SetDestination(player.position); // Follow player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectRange) // Lost player
        {
            currentState = State.Roaming;
            navMeshAgent.speed = roamingSpeed; // Reset speed for roaming
            SetNewRoamingPosition();
            UpdateVisualIndicator("?");
        }
    }

    private void StunBehavior()
    {
        stunTimer -= Time.deltaTime;

        if (stunTimer <= 0)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectRange)
            {
                currentState = State.Chasing;
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = chasingSpeed;
                UpdateVisualIndicator("!");
            }
            else
            {
                currentState = State.Roaming;
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = roamingSpeed;
                SetNewRoamingPosition();
                UpdateVisualIndicator("?");
            }
        }
    }

    public void Stun()
    {
        if (currentState != State.Stunned)
        {
            currentState = State.Stunned;
            stunTimer = stunDuration;

            navMeshAgent.isStopped = true; // Stop all movement
        }
    }

    public void SetInitialTarget(Transform target)
    {
        initialTarget = target;
    }

    private void MoveToInitialTarget()
    {
        navMeshAgent.SetDestination(initialTarget.position);

        if (Vector3.Distance(transform.position, initialTarget.position) < 0.5f)
        {
            reachedInitialTarget = true;
            currentState = State.Roaming;
            navMeshAgent.speed = roamingSpeed;
            SetNewRoamingPosition();
        }
    }

    private void UpdateVisualIndicator(string symbol)
    {
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