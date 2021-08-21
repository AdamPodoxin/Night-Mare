using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulInteractable : Interactable
{
    [SerializeField] private AudioClip collectClip;
    [SerializeField] private AudioSource globalSFX;

    [SerializeField] private bool isUnlocked = false;
    public bool IsUnlocked { get { return isUnlocked; } }

    [SerializeField] private GameObject lockObject;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (isUnlocked && !source.isPlaying) source.Play();
    }

    public override void Interact(PlayerInteraction playerInteraction)
    {
        if (isUnlocked)
        {
            playerInteraction.GetComponent<PlayerInventory>().CollectSoul();
            Destroy(gameObject);

            globalSFX.PlayOneShot(collectClip);
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
