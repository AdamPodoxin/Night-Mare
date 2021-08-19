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

    [Space]

    public bool isDying = false;

    public float _animateGrainIntensity;
    public float _animateSaturationValue;

    private bool isChasing = false;

    private LensDistortion distort;
    private ChromaticAberration aberration;
    private Grain grain;
    private ColorGrading colorGrading;

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
        ppVolume.profile.TryGetSettings(out grain);
        ppVolume.profile.TryGetSettings(out colorGrading);
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

        if (isDying)
        {
            grain.intensity.value = _animateGrainIntensity;
            colorGrading.saturation.value = _animateSaturationValue;
        }
    }

    public void FullDistort() { distort.intensity.value = distortAmount; }

    public void ToggleChase(bool isChasing)
    {
        this.isChasing = isChasing;
        aberration.intensity.value = isChasing ? chaseAberrationIntensity : normalAberrationIntensity;
    }
}
