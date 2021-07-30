using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class PlayerFootsteps : MonoBehaviour
{
    public GroundSound[] groundSounds;
    public GroundType currentGround;

    [Space]

    public float footstepCountdown = 0.5f;

    private AudioSource source;
    private CharacterController characterController;
    private Rigidbody rb;

    [SerializeField] private bool isWalking = false;
    private float footstepTimer = 0f;

    private AudioClip[] currentGroundSounds;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        footstepTimer = footstepCountdown;

        OnChangeGround(GroundType.Wood);
    }

    private void Update()
    {
        isWalking = characterController.velocity != Vector3.zero;

        if (isWalking)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f) PlaySound();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            OnChangeGround(collision.gameObject.GetComponent<Ground>().groundType);
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

    private void PlaySound()
    {
        AudioClip currentSound = currentGroundSounds[Random.Range(0, currentGroundSounds.Length)];
        source.PlayOneShot(currentSound);

        footstepTimer = footstepCountdown;
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
