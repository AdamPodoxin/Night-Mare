using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreNightmareBedInfoBox : InfoBox
{
    public GameObject bedInteractable;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        base.Interact(playerInteraction);
        bedInteractable.SetActive(true);
    }
}
