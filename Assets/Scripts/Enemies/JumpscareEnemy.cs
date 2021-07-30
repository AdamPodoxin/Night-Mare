using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareEnemy : MonoBehaviour
{
    public static JumpscareEnemy instance;

    public AudioClip[] sounds;

    private Animator anim;
    private AudioSource source;

    private int _prevIndex = -1;

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

        int index = Random.Range(0, sounds.Length);
        while (index == _prevIndex) { index = Random.Range(0, sounds.Length); }
        _prevIndex = index;

        source.PlayOneShot(sounds[index]);
    }
}
