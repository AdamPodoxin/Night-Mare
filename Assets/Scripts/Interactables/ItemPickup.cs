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

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }

    public override void Interact()
    {
        base.Interact();

        PlayerInventory.instance.PickupItem(item);
        Destroy(gameObject);
    }
}
