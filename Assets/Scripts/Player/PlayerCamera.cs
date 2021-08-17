using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerCamera : MonoBehaviour
{
    public PostProcessVolume ppVolume;

    public GameObject redOverlay;

    public float distortAmount = -69f;
    public float distortSpeed = 23f;

    private LensDistortion distort;

    private bool isInEyeball = false;
    public bool IsInEyeball
    {
        get { return isInEyeball; }
        set
        {
            isInEyeball = value;
            redOverlay.SetActive(value);
        }
    }

    private void Start()
    {
        ppVolume.profile.TryGetSettings(out distort);
    }

    private void Update()
    {
        if (isInEyeball)
        {
            if (distort.intensity.value > distortAmount)
            {
                distort.intensity.value -= distortSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (distort.intensity.value < 0f)
            {
                distort.intensity.value += distortSpeed * Time.deltaTime;
            }
        }
    }

    public void FullDistort() { distort.intensity.value = distortAmount; }
}
