using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBed : Interactable
{
    public GameObject animCam;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        animCam.SetActive(true);
    }
}
