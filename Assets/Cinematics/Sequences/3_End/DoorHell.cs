using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHell : MonoBehaviour
{
    public float spatialBlend = 0.98f;

    private AudioSource source;

    private bool isFading = false;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        source.spatialBlend = spatialBlend;

        if (isFading)
        {
            source.volume -= Time.deltaTime * 0.5f;
        }
    }

    public void StartFade()
    {
        isFading = true;
    }
}
