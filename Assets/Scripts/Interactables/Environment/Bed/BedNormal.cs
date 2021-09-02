using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

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

    public FirstPersonController normalFps;
    public FirstPersonController nightmareFps;

    [Space]

    public Animator blinkAnim;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        StartCoroutine(InteractCoroutine());
    }

    protected IEnumerator InteractCoroutine()
    {
        blinkAnim.Play("Blink_Fast");
        normalFps.enabled = false;

        yield return new WaitForSeconds(0.25f);

        normalFps.enabled = true;

        normalWorld.SetActive(false);
        nightmareWorld.SetActive(true);

        nightmarePlayer.position = normalPlayer.position;
        nightmarePlayer.rotation = normalPlayer.rotation;

        nightmareCamera.rotation = normalCamera.rotation;
        nightmareFps.cinemachineTargetPitch = normalFps.cinemachineTargetPitch;

        normalFps.input.move = Vector2.zero;
        normalFps.input.look = Vector2.zero;

        Item currentItem = normalInventory.currentItem;
        if (currentItem != null)
        {
            nightmareInventory.PickupItem(currentItem, currentItem.pickup.GetComponent<ItemPickup>(), true);
            normalInventory.HideCurrentItem();
        }
    }
}
