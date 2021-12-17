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

    public Light eyeLight;

    [Space]

    public Transform spawnDemonWaypoint;

    private bool isRotating = false;
    private bool isTrackingPlayer = false;
    private bool hasSpottedPlayer = false;

    private LayerMask ignoreLayers;

    private float rotateTimer = 0f;
    private int rotateDirection = 1;

    private float playerTrackTimer = 0f;

    private float xRot, yRot, zRot;
    private bool hasReachedMin, hasReachedMax;

    private AudioSource audioSource;
    private float originalVolume;

    private Animator anim;

    private Transform playerTransform;
    private DemonEnemy demon;
    private PlayerCamera playerCamera;
    private PlayerInventory playerInventory;

    private void Start()
    {
        ResetRotateTimer();
        ResetPlayerTimer();

        SetColors(normalColor);
        eyeLight.spotAngle = maxAngle;

        xRot = transform.eulerAngles.x;
        zRot = transform.eulerAngles.z;

        audioSource = GetComponent<AudioSource>();
        originalVolume = audioSource.volume;

        anim = GetComponent<Animator>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        demon = DemonEnemy.instance;
        playerCamera = FindObjectOfType<PlayerCamera>();
        playerInventory = FindObjectOfType<PlayerInventory>();

        ignoreLayers = demon.ignoreLayers;
    }

    private void Update()
    {
        yRot = transform.localEulerAngles.y;
        hasReachedMin = yRot <= minRotation;
        hasReachedMax = yRot >= maxRotation;

        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out RaycastHit hit, Mathf.Infinity, ~ignoreLayers))
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
                if (playerTrackTimer <= 0f) CallDemon(true);
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
        eyeLight.color = color;
    }

    public void PlayerEnterVision()
    {
        if (demon.State.Equals(GlobalEnums.DemonState.Killing))
            return;

        if (!isTrackingPlayer)
        {
            playerCamera.EnterEyeball();

            if (demon.gameObject.activeInHierarchy || (!demon.gameObject.activeInHierarchy && demon.isSummoning))
            {
                CallDemon(false);
                SetColors(trackingColor);
                hasSpottedPlayer = true;

                playerCamera.FullDistort();
            }
            else if (!playerInventory.currentArtifact.Equals(GlobalEnums.ArtifactType.Null))
            {
                CallDemon(false, true);
                SetColors(trackingColor);

                hasSpottedPlayer = true;

                playerCamera.FullDistort();
            }
            else
            {
                isTrackingPlayer = true;
                hasSpottedPlayer = true;

                SetColors(trackingColor);
                StopRotate();

                anim.speed = 0f;

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
        StartCoroutine(ResumeRotateCoroutine());

        anim.speed = 1f;

        playerCamera.ExitEyeball();
    }

    private IEnumerator ResumeRotateCoroutine()
    {
        yield return new WaitForSeconds(1f);
        if (!hasSpottedPlayer) StartRotate();
    }

    private IEnumerator SummonDemonCoroutine(bool useVoiceline, bool isCarryingArtifact)
    {
        demon.isSummoning = true;

        demon.TogglePlayerChase();

        yield return new WaitForSeconds(2f);

        demon.isSummoning = false;

        demon.gameObject.SetActive(true);
        demon.EyeballSummon(useVoiceline, isCarryingArtifact);
    }

    public void CallDemon(bool useVoiceline, bool isCarryingArtifact = false)
    {
        Vector3 playerDirection = playerTransform.forward.normalized;

        if (demon.gameObject.activeInHierarchy)
        {
            demon.EyeballGotPosition(playerTransform.position, playerDirection);
            demon.EyeballSummon(useVoiceline, isCarryingArtifact);
            demon.TogglePlayerChase();
        }
        else
        {
            if (demon.isSummoning)
            {
                demon.EyeballGotPosition(playerTransform.position, playerDirection);
            }
            else
            {
                demon.transform.position = spawnDemonWaypoint.position;
                demon.EyeballGotPosition(playerTransform.position, playerDirection);
                StartCoroutine(SummonDemonCoroutine(useVoiceline, isCarryingArtifact));
            }
        }

        JumpscareEnemy.instance.ActivateJumpscare();

        playerTrackTimer = playerTrackWaitTime;
        isTrackingPlayer = false;

        SetColors(normalColor);
        ResetPlayerTimer();
        StartRotate();
    }
}
