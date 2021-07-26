using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GlobalEnums;

public class DemonEnemy : MonoBehaviour
{
    public static DemonEnemy instance;

    public DemonState state;

    private NavMeshAgent agent;

    private Vector3 lastKnownPosition;
    private Vector3 lastKnownDirection;

    private float timer = 0f;
    private bool isTiming = true;

    private void Awake()
    {
        instance = this;

        agent = GetComponent<NavMeshAgent>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isTiming) timer += Time.deltaTime;
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

    public void SpottedPlayer(Vector3 position, Vector3 direction, bool useJumpscare)
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
}
