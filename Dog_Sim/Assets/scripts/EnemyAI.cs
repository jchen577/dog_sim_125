using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{   
    public float roamingSpeed = 2f;
    public float chasingSpeed = 4f;
    public float detectRange = 10f;

    private Transform player;
    private Vector3 roamPosition;
    private float roamRadius = 10f;
    private enum State
    {
        Roaming,
        Chasing
    }
    private State currentState;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = State.Roaming;
        SetNewRoamingPosition();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case State.Roaming:
                RoamingBehavior();
                CheckForPlayer();
                break;
            case State.Chasing:
                ChasingBehavior();
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
            currentState = State.Chasing;
        }
    }
    private void ChasingBehavior(){
        transform.position = Vector3.MoveTowards(transform.position, player.position, chasingSpeed * Time.deltaTime);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(distanceToPlayer > detectRange)
        {
            currentState = State.Roaming;
            SetNewRoamingPosition();
        }
    }
}
