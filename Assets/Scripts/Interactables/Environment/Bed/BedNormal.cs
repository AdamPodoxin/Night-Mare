using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedNormal : Interactable
{
    public GameObject normalWorld;
    public GameObject nightmareWorld;

    [Space]

    public Transform normalPlayer;
    public Transform nightmarePlayer;

    public Transform normalCamera;
    public Transform nightmareCamera;

    [Space]

    public PlayerInventory normalInventory;
    public PlayerInventory nightmareInventory;

    [Space]

    public CharacterController characterController;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        StartCoroutine(InteractCoroutine());
    }

    protected IEnumerator InteractCoroutine()
    {
        //Add blink animation later
        yield return null;

        normalWorld.SetActive(false);
        nightmareWorld.SetActive(true);

        nightmarePlayer.position = normalPlayer.position;
        nightmarePlayer.eulerAngles = normalPlayer.eulerAngles;

        nightmareCamera.eulerAngles = normalCamera.eulerAngles;

        Item currentItem = normalInventory.currentItem;
        if (currentItem != null)
        {
            nightmareInventory.PickupItem(currentItem, currentItem.pickup.GetComponent<ItemPickup>());
            normalInventory.HideCurrentItem();
        }
    }
}
