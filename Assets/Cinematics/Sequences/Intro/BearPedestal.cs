using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class BearPedestal : Interactable
{
    private bool isUnlocked = false;

    [SerializeField] private GameObject insertedArtifact;
    [SerializeField] private Material green;

    private PlayerInventory playerInventory;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        playerInventory = playerInteraction.GetComponent<PlayerInventory>();

        if (isUnlocked)
        {
            Notification.instance.DisplayNotification("Already inserted");
            return;
        }

        if (playerInventory != null)
        {
            if (playerInventory.currentArtifact.Equals(ArtifactType.Bear))
            {
                if (!isUnlocked)
                {
                    isUnlocked = true;
                    FindObjectOfType<IntroManager>().GaveGirlBear();

                    playerInventory.RemoveCurrentItem();

                    insertedArtifact.SetActive(true);
                    GetComponent<Renderer>().sharedMaterial = green;
                    GetComponent<AudioSource>().Play();

                    Destroy(this);
                }
            }
            else
            {
                Notification.instance.DisplayNotification("Must insert correct artifact");
            }
        }
    }
}
