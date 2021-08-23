using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreNightmareAnimationCamera : MonoBehaviour
{
    public GameObject player;
    public GameObject[] infoBoxes;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SwitchToPlayer()
    {
        player.SetActive(true);

        foreach (GameObject g in infoBoxes)
        {
            g.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
