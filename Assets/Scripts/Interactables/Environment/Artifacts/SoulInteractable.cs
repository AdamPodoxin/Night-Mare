using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulInteractable : Interactable
{
    [SerializeField] private bool isUnlocked = false;
    public bool IsUnlocked { get { return isUnlocked; } }

    [SerializeField] public GameObject lockObject;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        if (isUnlocked)
        {
            playerInteraction.GetComponent<PlayerInventory>().CollectSoul();
            Destroy(gameObject);
        }
        else
        {
            Notification.instance.DisplayNotification("Locked");
        }
    }

    public void Unlock()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            lockObject.SetActive(false);
        }
    }
}
