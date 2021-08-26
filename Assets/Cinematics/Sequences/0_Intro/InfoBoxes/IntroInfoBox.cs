using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroInfoBox : InfoBox
{
    private bool hasInteracted = false;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        base.Interact(playerInteraction);

        if (!hasInteracted)
        {
            hasInteracted = true;
            FindObjectOfType<IntroManager>().InfoBoxesInteracted++;
        }
    }
}
