using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEnemy : MonoBehaviour
{
    public float minRotation = -90f;
    public float maxRotation = 90f;

    [Space]

    public float rotateSpeed = 30f;
    public float rotateWaitTime = 3f;

    [Space]

    public float playerTrackWaitTime = 3f;

    [Space]

    public Color normalColor = Color.white;
    public Color trackingColor = Color.red;

    public Material lightConeMat;
    public Light eyeLight;

    private bool isRotating = false;
    private bool isTrackingPlayer = false;

    private float rotateTimer = 0f;
    private int rotateDirection = 1;

    private float playerTrackTimer = 0f;

    private float xRot, yRot, zRot;
    private bool hasReachedMin, hasReachedMax;

    private AudioSource audioSource;
    private float originalVolume;

    private Vector3 originalPosition;

    private void Start()
    {
        ResetRotateTimer();
        ResetPlayerTimer();

        SetColors(normalColor);

        xRot = transform.eulerAngles.x;
        zRot = transform.eulerAngles.z;

        audioSource = GetComponent<AudioSource>();
        originalVolume = audioSource.volume;

        originalPosition = transform.position;
    }

    private void Update()
    {
        yRot = transform.localEulerAngles.y;
        hasReachedMin = yRot <= minRotation;
        hasReachedMax = yRot >= maxRotation;

        if (isRotating)
        {
            transform.eulerAngles += Vector3.up * rotateDirection * rotateSpeed * Time.deltaTime;

            if (hasReachedMin)
            {
                StopRotate();
                SetYRot(minRotation + 1f);
            }
            else if (hasReachedMax)
            {
                StopRotate();
                SetYRot(maxRotation - 1f);
            }
        }
        else
        {
            if (isTrackingPlayer)
            {
                playerTrackTimer -= Time.deltaTime;
                if (playerTrackTimer <= 0f) CallDemon();
            }
            else
            {
                rotateTimer -= Time.deltaTime;

                if (rotateTimer <= 0f)
                {
                    ResetRotateTimer();
                    SwitchRotateDirection();
                    StartRotate();
                }
            }
        }

        if (!isTrackingPlayer)
        {
            audioSource.volume -= Time.deltaTime;
        }
    }

    private void ResetRotateTimer() { rotateTimer = rotateWaitTime; }
    private void ResetPlayerTimer() { playerTrackTimer = playerTrackWaitTime; }

    private void StopRotate() { isRotating = false; }
    private void StartRotate() { isRotating = true; }

    private void SwitchRotateDirection() { rotateDirection *= -1; }
    private void SetYRot(float rot) { transform.localEulerAngles = new Vector3(xRot, rot, zRot); }

    public void SetColors(Color color)
    {
        Color matColor = color;
        matColor.a = lightConeMat.color.a;

        lightConeMat.color = matColor;
        eyeLight.color = color;
    }

    public void PlayerEnterVision()
    {
        isTrackingPlayer = true;

        SetColors(trackingColor);
        StopRotate();

        audioSource.volume = originalVolume;
        audioSource.Play();
    }

    public void PlayerExitVision()
    {
        isTrackingPlayer = false;

        SetColors(normalColor);
        ResetPlayerTimer();
        StartRotate();
    }

    public void CallDemon()
    {
        //TEMP
        print("Called demon");

        playerTrackTimer = playerTrackWaitTime;
        isTrackingPlayer = false;

        SetColors(normalColor);
        ResetPlayerTimer();
        StartRotate();
    }
}
