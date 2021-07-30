using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool isOpen = false;

    [Space]

    public AudioClip openClip;
    public AudioClip closeClip;

    private Animator anim;
    private AudioSource source;

    private void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        base.Interact();

        if (isOpen) Close();
        else Open();
    }

    public void Open()
    {
        interactText = "Close";
        isOpen = true;

        anim.Play("Open");
        source.PlayOneShot(openClip);
    }

    public void Close()
    {
        interactText = "Open";
        isOpen = false;

        anim.Play("Close");
        source.PlayOneShot(closeClip);
    }
}
