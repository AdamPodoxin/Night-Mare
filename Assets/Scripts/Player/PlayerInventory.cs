using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    public Transform hand;

    private Item currentItem = null;
    private GameObject currentItemObject = null;

    private readonly Dictionary<string, GameObject> itemsPickedUp = new Dictionary<string, GameObject>();

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        instance = this;

        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Drop.performed += DoDropItem;
        playerInputActions.Player.Drop.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Drop.Disable();
    }

    private void DoDropItem(InputAction.CallbackContext obj)
    {
        DropCurrentItem();
    }

    public GameObject FindItemInHand(Item item)
    {
        for (int i = 0; i < hand.childCount; i++)
        {
            Transform currentChild = hand.GetChild(i);
            if (currentChild.name.Equals(item.prefab.name)) return currentChild.gameObject;
        }

        return null;
    }

    public void PickupItem(Item item)
    {
        if (currentItem != null) DropCurrentItem();
        currentItem = item;

        itemsPickedUp.TryGetValue(item.name, out currentItemObject);

        if (currentItemObject == null)
        {
            currentItemObject = Instantiate(currentItem.prefab, hand.position, Quaternion.identity);
            currentItemObject.transform.SetParent(hand);

            itemsPickedUp.Add(item.name, currentItemObject);
        }
        else
        {
            currentItemObject.SetActive(true);
        }
    }

    public void DropCurrentItem()
    {
        Instantiate(currentItem.pickup, hand.position, Quaternion.identity);

        currentItemObject.SetActive(false);
        currentItemObject = null;

        currentItem = null;
    }

    public void RemoveCurrentItem()
    {
        itemsPickedUp.Remove(currentItem.name);

        Destroy(currentItemObject);
        currentItemObject = null;

        currentItem = null;
    }
}
