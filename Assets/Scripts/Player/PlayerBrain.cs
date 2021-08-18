using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class PlayerBrain : MonoBehaviour
{
    public static PlayerBrain instance;

    public float deathTimer = 3f;

    [Space]

    [SerializeField] private FirstPersonController fps;
    [SerializeField] private PlayerFootsteps footsteps;
    [SerializeField] private PlayerInteraction interaction;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerCamera playerCamera;

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator DieCoroutine()
    {
        fps.enabled = false;
        footsteps.enabled = false;
        interaction.enabled = false;

        inventory.DropCurrentItem();
        inventory.enabled = false;

        playerCamera.enabled = false;

        yield return new WaitForSeconds(deathTimer);

        SceneManager.LoadSceneAsync("2_Game", LoadSceneMode.Single);
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }
}
