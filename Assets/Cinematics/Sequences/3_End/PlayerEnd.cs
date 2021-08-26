using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerEnd : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Choice_L"))
        {
            FindObjectOfType<EndManager>().MakeChoice("L");
            GetComponent<FirstPersonController>().enabled = false;
        }
        else if (other.CompareTag("Choice_R"))
        {
            FindObjectOfType<EndManager>().MakeChoice("R");
            GetComponent<FirstPersonController>().enabled = false;
        }
    }
}
