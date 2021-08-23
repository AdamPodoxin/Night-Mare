using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PreNightmareBed : Interactable
{
    public PreNightmareAnimationCamera animCam;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        animCam.StartLerp();
    }
}
