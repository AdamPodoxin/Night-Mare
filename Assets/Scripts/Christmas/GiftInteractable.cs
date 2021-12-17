using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftInteractable : Interactable
{
    [SerializeField] private AudioClip collectClip;
    [SerializeField] private AudioSource globalSFX;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        globalSFX.PlayOneShot(collectClip);
        FindObjectOfType<ChristmasInventory>().GiftsCollected += 1;

        Destroy(gameObject);
    }
}
