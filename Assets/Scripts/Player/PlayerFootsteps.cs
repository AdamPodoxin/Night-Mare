using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class PlayerFootsteps : MonoBehaviour
{
    public GroundSound[] groundSounds;
    public GroundType currentGround = GroundType.Null;

    public LayerMask ignoreLayers;

    [Space]

    public float moveSpeed = 5f;
    public float timerMultiplier = 2.5f;

    [Space]

    public float pushBallStrength = 375f;

    private AudioSource source;
    private CharacterController characterController;

    private bool isWalking = false;
    private float footstepTimer = 0f;

    private RaycastHit _hit;
    private string _raycastTag;

    private AudioClip[] currentGroundSounds;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();

        ResetTimer();
        OnChangeGround(GroundType.Wood);
    }

    private void Update()
    {
        isWalking = characterController.velocity != Vector3.zero;

        if (Physics.Raycast(transform.localPosition, Vector3.down, out _hit, 100f, ~ignoreLayers))
        {
            _raycastTag = _hit.collider.tag;

            if (_raycastTag.Equals("Ground"))
            {
                GroundType groundType = _hit.collider.GetComponent<Ground>().groundType;
                if (groundType != currentGround) OnChangeGround(groundType);
            }
        }

        if (isWalking)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f) PlaySound();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * pushBallStrength);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.forward * pushBallStrength / 2f);
        }
    }

    private AudioClip[] FindGroundSounds(GroundType groundType)
    {
        foreach (GroundSound g in groundSounds)
        {
            if (g.groundType.Equals(groundType)) return g.sounds;
        }

        return null;
    }

    private void ResetTimer()
    {
        footstepTimer = timerMultiplier / moveSpeed;
    }

    private void PlaySound()
    {
        AudioClip currentSound = currentGroundSounds[Random.Range(0, currentGroundSounds.Length)];
        source.PlayOneShot(currentSound);

        ResetTimer();
    }

    private void OnChangeGround(GroundType ground)
    {
        currentGround = ground;
        currentGroundSounds = FindGroundSounds(ground);
    }
}

[System.Serializable]
public class GroundSound
{
    public GroundType groundType;
    public AudioClip[] sounds;
}
