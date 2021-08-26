using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHell : MonoBehaviour
{
    public float spatialBlend = 0.98f;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        source.spatialBlend = spatialBlend;
    }
}
