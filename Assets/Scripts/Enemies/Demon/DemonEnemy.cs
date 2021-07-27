using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GlobalEnums;

public class DemonEnemy : MonoBehaviour
{
    public static DemonEnemy instance;

    public DemonState state;

    public float acceptableStoppingDistance = 1.2f;

    private NavMeshAgent agent;

    private Transform playerTransform;
    private float playerMoveSpeed;

    private Vector3 lastKnownPosition;
    private Vector3 lastKnownDirection;

    private Vector3 playerFoundPosition;
    private Vector3 playerLostPosition;

    private float timer = 0f;
    private bool isTiming = true;

    private RaycastHit _hit;
    private bool _isChasingPlayer;
    private Vector3 _navTargetPosition;

    //TEMP
    Vector3 PREDICTED;

    private void Awake()
    {
        instance = this;

        agent = GetComponent<NavMeshAgent>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMoveSpeed = FindObjectOfType<StarterAssets.FirstPersonController>().MoveSpeed;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isTiming) timer += Time.deltaTime;

        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out _hit, Mathf.Infinity, ~LayerMask.GetMask("Enemy")))
        {
            if (_hit.collider.CompareTag("Player"))
            {
                if (!state.Equals(DemonState.Chasing)) OnFoundPlayer();

                lastKnownPosition = playerTransform.position;
                agent.SetDestination(lastKnownPosition);
            }
            else
            {
                if (state.Equals(DemonState.Chasing)) OnLostPlayer();
            }
        }
        else
        {
            if (state.Equals(DemonState.Chasing)) OnLostPlayer();
        }

        if (state.Equals(DemonState.Travelling))
        {
            if (Vector3.SqrMagnitude(_navTargetPosition - transform.position) <= acceptableStoppingDistance && !state.Equals(DemonState.Searching))
            {
                if (_isChasingPlayer)
                {
                    OnReachedLastKnownPosition();
                }
                else
                {
                    OnReachedWaypoint();
                }
            }
        }
    }

    private void StartTimer()
    {
        timer = 0f;
        isTiming = true;
    }

    private void StopTimer()
    {
        isTiming = false;
    }

    private Vector3 CalculatePlayerDirection(Vector3 position1, Vector3 position2)
    {
        return new Vector3(position2.x - position1.x, 0f, position2.z - position1.z).normalized;
    }

    private void OnFoundPlayer()
    {
        state = DemonState.Chasing;
        StopTimer();

        playerFoundPosition = playerTransform.position;

        _isChasingPlayer = true;
        print("Found");
    }

    private void OnLostPlayer()
    {
        state = DemonState.Travelling;
        StartTimer();

        playerLostPosition = _navTargetPosition = playerTransform.position;
        lastKnownDirection = CalculatePlayerDirection(playerFoundPosition, playerLostPosition);

        print("Lost");
    }

    private void OnReachedLastKnownPosition()
    {
        state = DemonState.Searching;
        StopTimer();

        Vector3 predictedPosition = PREDICTED = PredictPosition();

        print("Reached last known position");
    }

    private Vector3 PredictPosition()
    {
        float predictedDistance = playerMoveSpeed * Mathf.Log(timer, 2);
        return lastKnownPosition + lastKnownDirection * predictedDistance; ;
    }

    private void OnReachedWaypoint()
    {
        print("Reached waypoint");
    }

    public void GotPlayerPosition(Vector3 position, Vector3 direction, bool useJumpscare)
    {
        lastKnownPosition = _navTargetPosition = position;
        lastKnownDirection = direction;

        agent.SetDestination(lastKnownPosition);
        state = DemonState.Travelling;
        _isChasingPlayer = true;

        StartTimer();

        if (useJumpscare)
        {
            //Jumpscare
        }
    }

    //TEMP
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(PREDICTED, Vector3.one * 0.5f);
    }
}
