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

    public float lerpSpeed = 10f;
    public float chaseDistortAmount = -30f;

    public float normalAberrationIntensity = 0.3f;
    public float chaseAberrationIntensity = 1f;

    public float _animateGrainIntensity;
    public float _animateSaturationValue;

    private bool isChasing = false;

    private bool isDying = false;
    private bool isAnimatingPosition = false;

    private Transform demonHandTransform;
    private Transform demonEyeTransform;

    private LensDistortion distort;
    private ChromaticAberration aberration;
    private Grain grain;
    private ColorGrading colorGrading;

    private bool isInEyeball = false;

    private int _eyeballsAmount = 0;

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

            Quaternion lookAtEye = Quaternion.LookRotation(demonEyeTransform.position - transform.position - Vector3.up * 0.3f);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtEye, lerpSpeed * Time.deltaTime / 5f);

            if (isAnimatingPosition)
                transform.position = Vector3.Lerp(transform.position, demonHandTransform.position, lerpSpeed * Time.deltaTime);
        }
    }

    public void EnterEyeball()
    {
        _eyeballsAmount++;

        isInEyeball = true;
        redOverlay.SetActive(true);
    }

    public void ExitEyeball()
    {
        _eyeballsAmount--;

        if (_eyeballsAmount <= 0)
        {
            isInEyeball = false;
            redOverlay.SetActive(false);

            _eyeballsAmount = 0;
        }
    }

    public void FullDistort() { distort.intensity.value = distortAmount; }

    public void ToggleChase(bool isChasing)
    {
        this.isChasing = isChasing;
        aberration.intensity.value = isChasing ? chaseAberrationIntensity : normalAberrationIntensity;
    }

    public void StartDeath()
    {
        demonHandTransform = DemonEnemy.instance.handTransform;
        demonEyeTransform = DemonEnemy.instance.eyeHeightTransform;

        isDying = true;
    }

    public void StartAnimatingPosition()
    {
        isAnimatingPosition = true;
    }
}
