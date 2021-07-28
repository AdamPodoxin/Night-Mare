using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareManager : MonoBehaviour
{
    public static NightmareManager instance;

    private GameObject[] waypoints;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

    public Transform FindNearestWaypoint(Vector3 position)
    {
        Transform nearest = waypoints[0].transform;

        foreach (GameObject waypoint in waypoints)
        {
            if ((waypoint.transform.position - position).sqrMagnitude < (nearest.position - position).sqrMagnitude)
            {
                nearest = waypoint.transform;
            }
        }

        return nearest;
    }

    public Transform[] FindWaypointsInRadius(Vector3 position, float radius)
    {
        List<Transform> waypointsInRadius = new List<Transform>();

        foreach (GameObject waypoint in waypoints)
        {
            if (Vector3.Distance(waypoint.transform.position, position) <= radius)
            {
                waypointsInRadius.Add(waypoint.transform);
            }
        }

        return waypointsInRadius.ToArray();
    }
}
