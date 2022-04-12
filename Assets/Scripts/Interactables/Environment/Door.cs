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

    private bool isInAnimation = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (isInAnimation)
        {
            if (isOpen) Open();
            else Close();
        }
    }

    public override void Interact(PlayerInteraction playerInteraction)
    {
        if (isInAnimation) return;

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

    public void AnimatingTrue() => isInAnimation = true; 
    public void AnimatingFalse() => isInAnimation = false;
}
