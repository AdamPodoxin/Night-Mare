using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBoyInfoBox : InfoBox
{
    private bool hasInteracted = false;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        base.Interact(playerInteraction);

        FindObjectOfType<IntroManager>().InteractWithBoy();
        if (!hasInteracted)
        {
            hasInteracted = true;
            FindObjectOfType<IntroManager>().InfoBoxesInteracted++;
        }
    }
}
