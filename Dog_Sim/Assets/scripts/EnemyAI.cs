using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    public Transform initialTarget; // Store waypoint assigned by the spawner
    private bool reachedInitialTarget = false;
    public float roamingSpeed = 2f;
    public float chasingSpeed = 4f;
    public float detectRange = 10f;
    public float alertTime = 1.5f; // Time spent in Alerting state
    public float stunDuration = 2f; // Duration of the stun

    public GameObject visualIndicator; // Assign a child object for visual indicator

    public Vector3 storeCenter; // Center of the store area
    public Vector3 storeSize;   // Size of the store area (width, depth)

    private Transform player;
    private Vector3 roamPosition;
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
        if (currentState != State.Stunned)
        {
            currentState = State.Stunned;
            stunTimer = stunDuration;
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

    public void SetInitialTarget(Transform target)
    {
        initialTarget = target;
    }

    private void MoveToInitialTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, initialTarget.position, roamingSpeed * Time.deltaTime);
        transform.LookAt(initialTarget.position);

        if (Vector3.Distance(transform.position, initialTarget.position) < 0.5f)
        {
            reachedInitialTarget = true;
            currentState = State.Roaming;
            SetNewRoamingPosition();
        }
    }
}
