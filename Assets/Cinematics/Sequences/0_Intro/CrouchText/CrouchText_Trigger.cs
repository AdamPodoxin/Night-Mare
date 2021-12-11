using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchText_Trigger : MonoBehaviour
{
    public GameObject crouchText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            crouchText.SetActive(true);
            Destroy(gameObject);
        }
    }
}
