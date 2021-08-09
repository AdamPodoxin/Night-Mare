using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class PlayerFootsteps : MonoBehaviour
{
    public GroundSound[] groundSounds;
    public GroundType currentGround = GroundType.Null;

    [Space]

    public float footstepCountdown = 0.5f;

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

        footstepTimer = footstepCountdown;

        OnChangeGround(GroundType.Wood);
    }

    private void Update()
    {
        isWalking = characterController.velocity != Vector3.zero;

        if (Physics.Raycast(transform.localPosition, Vector3.down, out _hit, 100f))
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
