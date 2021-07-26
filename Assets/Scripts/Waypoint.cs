using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {

        Gizmos.DrawIcon(transform.position + Vector3.up, "sv_icon_dot10_pix16_gizmo", true);
    }
}
