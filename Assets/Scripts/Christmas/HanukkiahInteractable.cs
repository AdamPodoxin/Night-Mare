using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanukkiahInteractable : Interactable
{
    public AudioClip havaNagila;
    public AudioSource doorSource;

    [Space]

    public AudioClip interactClip;
    public AudioSource globalSFX;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        doorSource.Stop();
        doorSource.clip = havaNagila;
        doorSource.Play();

        globalSFX.PlayOneShot(interactClip);

        Notification.instance.DisplayNotification("Did it do anything...?");

        Destroy(gameObject);
    }
}
