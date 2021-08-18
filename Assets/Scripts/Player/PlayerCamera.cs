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

    [Space]

    public float chaseDistortAmount = -30f;

    public float normalAberrationIntensity = 0.3f;
    public float chaseAberrationIntensity = 1f;

    private bool isChasing = false;

    private LensDistortion distort;
    private ChromaticAberration aberration;

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
        ppVolume.profile.TryGetSettings(out aberration);
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
            if (distort.intensity.value < (isChasing ? chaseDistortAmount : 0f))
            {
                distort.intensity.value += distortSpeed * Time.deltaTime;
            }
        }
    }

    public void FullDistort() { distort.intensity.value = distortAmount; }

    public void ToggleChase(bool isChasing)
    {
        this.isChasing = isChasing;
        aberration.intensity.value = isChasing ? chaseAberrationIntensity : normalAberrationIntensity;
    }
}
