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

    [Space]

    public float droppingForce = 150f;

    [Space]

    public Item currentItem = null;
    public ArtifactType currentArtifact;

    [Space]

    [SerializeField] private int soulsCollected = 0;
    [SerializeField] private int artifactsCollected = 0;

    [Space]

    public Image soulsUI;
    public Text dropText;

    [Space]

    public bool useVoicelines = false;
    public AudioSource voiceSource;
    public AudioClip a1Son, a1Daughter, a1Wife, a2, a3;

    private GameObject currentItemObject = null;

    private readonly Dictionary<string, GameObject> itemsPickedUp = new Dictionary<string, GameObject>();

    private PlayerInputActions playerInputActions;

    private GameSubtitles gameSubtitles;

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
        dropText.gameObject.SetActive(false);
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

    public bool PickupItem(Item item, ItemPickup itemPickup, bool switchWorlds = false)
    {
        if (!currentArtifact.Equals(ArtifactType.Null))
        {
            Notification.instance.DisplayNotification("Cannot drop artifacts");
            return false;
        }

        if (currentItem != null) DropCurrentItem();
        currentItem = item;

        itemsPickedUp.TryGetValue(item.name, out currentItemObject);

        if (currentItemObject == null)
        {
            currentItemObject = Instantiate(currentItem.prefab, hand.position, Quaternion.identity);
            currentItemObject.transform.SetParent(hand);
            currentItemObject.transform.localRotation = currentItem.prefab.transform.localRotation;

            itemsPickedUp.Add(currentItem.name, currentItemObject);
        }
        else
        {
            currentItemObject.SetActive(true);
        }

        try
        {
            currentArtifact = itemPickup.GetComponent<ArtifactPickup>().artifactType;

            if (!switchWorlds)
            {
                artifactsCollected++;

                if (useVoicelines)
                {
                    try
                    {
                        if (gameSubtitles == null) gameSubtitles = FindObjectOfType<GameSubtitles>();

                        AudioClip playClip = null;
                        switch (artifactsCollected)
                        {
                            case 1:
                                switch (currentArtifact)
                                {
                                    case ArtifactType.Car:
                                        playClip = a1Son;
                                        gameSubtitles.ShowSubtitles("Okay, gotta wake up and put it on my son's bed.", 5f);
                                        break;
                                    case ArtifactType.Bear:
                                        playClip = a1Daughter;
                                        gameSubtitles.ShowSubtitles("Okay, gotta wake up and put it on my daughter's bed.", 5f);
                                        break;
                                    case ArtifactType.Flowers:
                                        playClip = a1Wife;
                                        gameSubtitles.ShowSubtitles("Okay, gotta wake up and put it on my wife's bed.", 5f);
                                        break;
                                }
                                break;
                            case 2:
                                playClip = a2;
                                gameSubtitles.ShowSubtitles("I need to keep going.");
                                break;
                            case 3:
                                playClip = a3;
                                gameSubtitles.ShowSubtitles("Almost done.");
                                break;
                        }
                        voiceSource.PlayOneShot(playClip);
                    }
                    catch { }
                }
            }
        }
        catch
        {
            currentArtifact = ArtifactType.Null;
        }

        if (currentArtifact.Equals(ArtifactType.Null))
        {
            dropText.gameObject.SetActive(true);
            dropText.text = "Drop " + currentItem.name;
        }

        return true;
    }

    public void DropCurrentItem()
    {
        if (currentItem == null) return;

        if (!currentArtifact.Equals(ArtifactType.Null))
        {
            Notification.instance.DisplayNotification("Cannot drop artifacts");
            return;
        }

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

    public void RemoveCurrentItem()
    {
        if (currentItem == null) return;

        itemsPickedUp.Remove(currentItem.name);
        Destroy(currentItemObject);
        currentItemObject = null;

        currentItem = null;
        currentArtifact = ArtifactType.Null;

        dropText.gameObject.SetActive(false);
    }

    public void CollectSoul()
    {
        soulsCollected++;
        soulsUI.fillAmount = soulsCollected / 3f;

        if (soulsCollected >= 3)
        {
            FindObjectOfType<NightmareManager>().StartCollapse();
        }
    }
}
