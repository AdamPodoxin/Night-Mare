using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform follow;
    public Vector3 offset;

    private void Update()
    {
        transform.position = follow.position + offset;
    }
}
