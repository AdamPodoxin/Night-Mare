using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonEnemy : MonoBehaviour
{
    public static DemonEnemy instance;

    private NavMeshAgent agent;

    private void Awake()
    {
        instance = this;

        agent = GetComponent<NavMeshAgent>();

        gameObject.SetActive(false);
    }
}
