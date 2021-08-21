using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class ArtifactPortal : Interactable
{
    public ArtifactType artifactType;

    [SerializeField] private bool isUnlocked = false;

    [SerializeField] private GameObject insertedArtifact;
    [SerializeField] private SoulInteractable soul;

    private PlayerInventory playerInventory;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        playerInventory = playerInteraction.GetComponent<PlayerInventory>();

        if (isUnlocked)
        {
            Notification.instance.DisplayNotification("Already unlocked");
            return;
        }

        if (playerInventory != null)
        {
            if (playerInventory.currentArtifact.Equals(artifactType))
            {
                if (!isUnlocked)
                {
                    isUnlocked = true;

                    playerInventory.RemoveCurrentItem();
                    soul.Unlock();

                    insertedArtifact.SetActive(true);
                    GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                Notification.instance.DisplayNotification("Must insert correct artifact");
            }
        }
    }
}
