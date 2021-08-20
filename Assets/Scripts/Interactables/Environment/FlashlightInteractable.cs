using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightInteractable : Interactable
{
    public GameObject playerFlashlight;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        playerFlashlight.SetActive(true);
        gameObject.SetActive(false);
    }
}
