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
            if (Vector3.SqrMagnitude(lastKnownPosition - transform.position) <= acceptableStoppingDistance && !state.Equals(DemonState.Searching)) OnReachedLastKnownPosition();
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

    public static Vector3 CalculatePlayerDirection(Vector3 position1, Vector3 position2)
    {
        return new Vector3(position2.x - position1.x, 0f, position2.z - position1.z).normalized;
    }

    public void GotPlayerPosition(Vector3 position, Vector3 direction, bool useJumpscare)
    {
        lastKnownPosition = position;
        lastKnownDirection = direction;

        agent.SetDestination(lastKnownPosition);
        state = DemonState.Travelling;

        StartTimer();

        if (useJumpscare)
        {
            //Jumpscare
        }
    }

    public void OnFoundPlayer()
    {
        state = DemonState.Chasing;
        StopTimer();

        playerFoundPosition = playerTransform.position;

        print("Found");
    }

    public void OnLostPlayer()
    {
        state = DemonState.Travelling;
        StartTimer();

        playerLostPosition = playerTransform.position;
        lastKnownDirection = CalculatePlayerDirection(playerFoundPosition, playerLostPosition);

        print("Lost");
    }

    public void OnReachedLastKnownPosition()
    {
        state = DemonState.Searching;
        StopTimer();

        float predictedDistance = playerMoveSpeed * timer;
        Vector3 predictedPosition = lastKnownPosition + lastKnownDirection * predictedDistance;
        print(predictedPosition);

        print("Reached last known position");
    }
}
