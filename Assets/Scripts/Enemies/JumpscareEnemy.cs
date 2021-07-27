using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareEnemy : MonoBehaviour
{
    public static JumpscareEnemy instance;

    public AudioClip[] sounds;

    private Animator anim;
    private AudioSource source;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public void ActivateJumpscare()
    {
        anim.Play("Activate");
        source.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }
}
