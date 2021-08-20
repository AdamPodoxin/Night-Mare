using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedNightmare : Interactable
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

    public DemonEnemy demon;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        StartCoroutine(InteractCoroutine());
    }

    protected IEnumerator InteractCoroutine()
    {
        //Add blink animation later
        yield return null;

        nightmareWorld.SetActive(false);
        normalWorld.SetActive(true);

        normalPlayer.position = nightmarePlayer.position;
        normalPlayer.eulerAngles = nightmarePlayer.eulerAngles;

        normalCamera.eulerAngles = nightmareCamera.eulerAngles;

        Item currentItem = nightmareInventory.currentItem;
        if (currentItem != null)
        {
            normalInventory.PickupItem(currentItem, currentItem.pickup.GetComponent<ItemPickup>());
            nightmareInventory.HideCurrentItem();
        }

        demon.SwitchWorlds();
    }
}
