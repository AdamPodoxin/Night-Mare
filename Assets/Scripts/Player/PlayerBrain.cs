using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StarterAssets;
using static GlobalEnums;

public class PlayerBrain : MonoBehaviour
{
    public float deathTimer = 3f;
    public bool isSwitchingWorlds = false;

    [Space]

    [SerializeField] private FirstPersonController fps;
    [SerializeField] private PlayerFootsteps footsteps;
    [SerializeField] private PlayerInteraction interaction;
    [SerializeField] private PlayerInventory inventory;

    [Space]

    [SerializeField] private Animator camAnim;
    [SerializeField] private Animator blinkAnim;
    [SerializeField] private GameObject redOverlay;
    [SerializeField] private GameObject flash;
    [SerializeField] private GameObject loadingText;

    [Space]

    [SerializeField] private AudioSource globalSFX;
    [SerializeField] private AudioClip collapseInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collapse"))
        {
            StartCoroutine(CollapseCoroutine());
        }
    }

    private IEnumerator CollapseCoroutine()
    {
        redOverlay.SetActive(true);
        RenderSettings.fog = true;

        globalSFX.PlayOneShot(collapseInside);

        fps.enabled = false;
        footsteps.enabled = false;
        interaction.enabled = false;

        if (inventory.currentArtifact.Equals(ArtifactType.Null)) inventory.DropCurrentItem();
        inventory.enabled = false;

        yield return new WaitForSeconds(1f);

        flash.SetActive(true);

        yield return new WaitForSeconds(1f);

        loadingText.SetActive(true);
        SceneManager.LoadSceneAsync("3_End", LoadSceneMode.Single);
    }

    private IEnumerator DieCoroutine()
    {
        float blinkTimer = 1f;

        fps.enabled = false;
        footsteps.enabled = false;
        interaction.enabled = false;

        if (inventory.currentArtifact.Equals(ArtifactType.Null)) inventory.DropCurrentItem();
        inventory.enabled = false;

        camAnim.Play("Dying");

        yield return new WaitForSeconds(deathTimer - blinkTimer);
        blinkAnim.Play("Blink_Slow_Close");
        yield return new WaitForSeconds(blinkTimer);

        SceneManager.LoadSceneAsync("2_Game", LoadSceneMode.Single);
    }

    public void Die()
    {
        if (isSwitchingWorlds) return;
        StartCoroutine(DieCoroutine());
    }
}
