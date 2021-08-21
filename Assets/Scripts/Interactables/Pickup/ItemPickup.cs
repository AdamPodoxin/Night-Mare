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
        bool hasPickedUp = playerInteraction.GetComponent<PlayerInventory>().PickupItem(item, this);
        if (hasPickedUp) Destroy(gameObject);
    }
}
