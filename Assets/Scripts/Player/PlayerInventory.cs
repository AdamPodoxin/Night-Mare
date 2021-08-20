using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static GlobalEnums;

public class PlayerInventory : MonoBehaviour
{
    public Transform hand;
    public Transform currentWorld;

    public ArtifactType currentArtifact;

    [Space]

    public Text dropText;

    [Space]

    public float droppingForce = 150f;

    public Item currentItem = null;
    private GameObject currentItemObject = null;

    private readonly Dictionary<string, GameObject> itemsPickedUp = new Dictionary<string, GameObject>();

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
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

    public void PickupItem(Item item, ItemPickup itemPickup)
    {
        if (currentItem != null) DropCurrentItem();
        currentItem = item;

        itemsPickedUp.TryGetValue(item.name, out currentItemObject);

        if (currentItemObject == null)
        {
            currentItemObject = Instantiate(currentItem.prefab, hand.position, Quaternion.identity);
            currentItemObject.transform.SetParent(hand);

            itemsPickedUp.Add(currentItem.name, currentItemObject);
        }
        else
        {
            currentItemObject.SetActive(true);
        }

        try
        {
            currentArtifact = itemPickup.GetComponent<ArtifactPickup>().artifactType;
        }
        catch
        {
            currentArtifact = ArtifactType.Null;
        }

        dropText.gameObject.SetActive(true);
        dropText.text = "Drop " + currentItem.name;
    }

    public void DropCurrentItem()
    {
        if (currentItem == null) return;

        Transform spawnedItem = Instantiate(currentItem.pickup, transform.position + transform.forward, Quaternion.identity).transform;
        spawnedItem.SetParent(currentWorld);

        Rigidbody itemRb = spawnedItem.GetComponent<Rigidbody>();
        if (itemRb != null) itemRb.AddForce(transform.forward * droppingForce);

        currentItemObject.SetActive(false);
        currentItemObject = null;

        currentItem = null;
        currentArtifact = ArtifactType.Null;

        dropText.gameObject.SetActive(false);
    }

    public void HideCurrentItem()
    {
        if (currentItem == null) return;

        currentItemObject.SetActive(false);
        currentItemObject = null;

        currentItem = null;
        currentArtifact = ArtifactType.Null;

        dropText.gameObject.SetActive(false);
    }
}
