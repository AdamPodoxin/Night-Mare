using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PreNightmareBed : Interactable
{
    public GameObject normalWorld;
    public GameObject nightmareWorld;
    public Animator blinkAnim;

    public override void Interact(PlayerInteraction playerInteraction)
    {
        StartCoroutine(InteractCoroutine());
    }

    protected IEnumerator InteractCoroutine()
    {
        blinkAnim.Play("Blink_Fast");

        yield return new WaitForSeconds(0.25f);

        normalWorld.SetActive(false);
        nightmareWorld.SetActive(true);
    }
}
