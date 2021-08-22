using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

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

    public FirstPersonController normalFps;
    public FirstPersonController nightmareFps;

    [Space]

    public DemonEnemy demon;

    [Space]

    public Animator blinkAnim;

    [SerializeField] private PlayerBrain playerBrain;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        if (demon.gameObject.activeInHierarchy)
        {
            Notification.instance.DisplayNotification("Cannot wake up while being hunted");
        }
        else
        {
            StartCoroutine(InteractCoroutine());
        }
    }

    protected IEnumerator InteractCoroutine()
    {
        blinkAnim.Play("Blink_Fast");
        nightmareFps.enabled = false;

        playerBrain.isSwitchingWorlds = true;

        yield return new WaitForSeconds(0.25f);

        playerBrain.isSwitchingWorlds = false;

        nightmareFps.enabled = true;

        nightmareWorld.SetActive(false);
        normalWorld.SetActive(true);

        normalPlayer.position = nightmarePlayer.position;
        normalPlayer.rotation = nightmarePlayer.rotation;

        normalCamera.rotation = nightmareCamera.rotation;
        normalFps.cinemachineTargetPitch = nightmareFps.cinemachineTargetPitch;

        nightmareFps.input.move = Vector2.zero;
        nightmareFps.input.look = Vector2.zero;

        Item currentItem = nightmareInventory.currentItem;
        if (currentItem != null)
        {
            normalInventory.PickupItem(currentItem, currentItem.pickup.GetComponent<ItemPickup>());
            nightmareInventory.HideCurrentItem();
        }

        demon.SwitchWorlds();
    }
}
