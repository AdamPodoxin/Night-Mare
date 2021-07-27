using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEnemy : MonoBehaviour
{
    public float maxAngle = 50f;
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
    private bool hasSpottedPlayer = false;

    private float rotateTimer = 0f;
    private int rotateDirection = 1;

    private float playerTrackTimer = 0f;

    private float xRot, yRot, zRot;
    private bool hasReachedMin, hasReachedMax;

    private AudioSource audioSource;
    private float originalVolume;

    private Transform playerTransform;
    private DemonEnemy demon;

    private void Start()
    {
        ResetRotateTimer();
        ResetPlayerTimer();

        SetColors(normalColor);
        eyeLight.spotAngle = maxAngle + 3f;

        xRot = transform.eulerAngles.x;
        zRot = transform.eulerAngles.z;

        audioSource = GetComponent<AudioSource>();
        originalVolume = audioSource.volume;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        demon = DemonEnemy.instance;
    }

    private void Update()
    {
        yRot = transform.localEulerAngles.y;
        hasReachedMin = yRot <= minRotation;
        hasReachedMax = yRot >= maxRotation;

        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out RaycastHit hit, Mathf.Infinity, ~LayerMask.GetMask("Enemy")))
        {
            if (hit.collider.CompareTag("Player") && Vector3.Angle(transform.forward, (playerTransform.position - transform.position).normalized) <= maxAngle / 2f)
            {
                if (!hasSpottedPlayer)
                {
                    PlayerEnterVision();
                }
            }
            else if (hasSpottedPlayer)
            {
                PlayerExitVision();
            }
        }
        else if (hasSpottedPlayer)
        {
            PlayerExitVision();
        }

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

    private void OnDrawGizmos()
    {
        float rayRange = 10.0f;
        float halfFOV = maxAngle / 2f;

        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Quaternion upRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.right);
        Quaternion downRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.right);

        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Vector3 upRayDirection = upRayRotation * transform.forward;
        Vector3 downRayDirection = downRayRotation * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, upRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, downRayDirection * rayRange);
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
        if (!isTrackingPlayer)
        {
            if (demon.gameObject.activeInHierarchy)
            {
                CallDemon();
                SetColors(trackingColor);
                hasSpottedPlayer = true;
            }
            else
            {
                isTrackingPlayer = true;
                hasSpottedPlayer = true;

                SetColors(trackingColor);
                StopRotate();

                audioSource.volume = originalVolume;
                audioSource.Play();
            }
        }
    }

    public void PlayerExitVision()
    {
        isTrackingPlayer = false;
        hasSpottedPlayer = false;

        SetColors(normalColor);
        ResetPlayerTimer();
        StartRotate();
    }

    public void CallDemon()
    {
        Vector3 playerDirection = playerTransform.forward.normalized;

        demon.gameObject.SetActive(true);
        demon.GotPlayerPosition(playerTransform.position, playerDirection, true);

        playerTrackTimer = playerTrackWaitTime;
        isTrackingPlayer = false;

        SetColors(normalColor);
        ResetPlayerTimer();
        StartRotate();
    }
}
