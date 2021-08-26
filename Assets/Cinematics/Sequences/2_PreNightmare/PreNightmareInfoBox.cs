using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreNightmareInfoBox : InfoBox
{
    public GameObject artifactPortal;

    private bool hasInteracted = false;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        base.Interact(playerInteraction);

        if (!hasInteracted)
        {
            hasInteracted = true;
            FindObjectOfType<PreNightmareManager>().InfoBoxesInteracted++;
        }

        if (!isReading)
        {
            GetComponent<Collider>().enabled = false;
            Destroy(this);

            artifactPortal.SetActive(true);
        }
    }
}
