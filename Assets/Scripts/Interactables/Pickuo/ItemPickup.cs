using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemPickup : Interactable
{
    public Item item;

    private void Start()
    {
        interactText = "Pick up " + item.name;
    }

    public override void Interact(PlayerInteraction playerInteraction)
    {
        playerInteraction.GetComponent<PlayerInventory>().PickupItem(item, this);
        Destroy(gameObject);
    }
}
