using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private Animator camAnim;
    [SerializeField] private Animator blinkAnim;

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
