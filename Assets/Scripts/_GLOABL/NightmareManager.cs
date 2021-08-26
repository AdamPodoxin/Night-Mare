using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using StarterAssets;

public class NightmareManager : MonoBehaviour
{
    public static NightmareManager instance;

    [SerializeField] private Animator blinkAnim;

    [Space]

    [SerializeField] private GameObject enemiesParent;
    [SerializeField] private GameObject collapseRed;

    [Space]

    [SerializeField] private PlayerCamera playerCam;
    [SerializeField] private FirstPersonController fps;

    [Space]

    [SerializeField] private BedNightmare bed;

    private GameObject[] waypoints;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        blinkAnim.Play("Blink_Slow_Open");
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

        //Sorts by ascending order based on distance from position
        waypointsInRadius = waypointsInRadius.OrderBy(w => Vector3.SqrMagnitude(w.position - position)).ToList();
        return waypointsInRadius.ToArray();
    }

    public void StartCollapse()
    {
        enemiesParent.SetActive(false);
        collapseRed.SetActive(true);

        playerCam.ToggleChase(false);
        playerCam.NoDistort();
        fps.ToggleChase(false);

        FindObjectOfType<BedNightmare>().isDreamCollapsing = true;
    }
}
